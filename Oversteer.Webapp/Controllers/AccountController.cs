using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Common;
using Oversteer.Helpers;
using Oversteer.Models;
using Oversteer.Webapp.Data;
using Oversteer.Webapp.Services;
using Token = Oversteer.Helpers.Token;

namespace Oversteer.Webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _emailSender;
        private readonly ITokenService _tokenService;

        private string LoginProvider { get; set; } = string.Empty;
        private string Email { get; set; } = string.Empty;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db, IEmailSender emailSender, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
            _emailSender = emailSender;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] dynamic message)
        {
            RegisterDto registerModel = JsonConvert.DeserializeObject<RegisterDto>(message.ToString());

            RegisterResult registerResult = new RegisterResult();

            var user = new ApplicationUser { UserName = registerModel.Email, Email = registerModel.Email };
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code = code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(registerModel.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                registerResult.Successful = true;
            }
            else
            {
                registerResult.Successful = false;
                foreach(var error in result.Errors)
                {
                    registerResult.Errors.Add(error.Code = ": " + error.Description);
                }
            }

            return Ok(registerResult);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            LoginViewModel loginViewModel = new LoginViewModel();

            var user = await _userManager.FindByNameAsync(userForAuthentication.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            string token = Helpers.Token.Generate(user.Email, null);

            RefreshToken refreshToken = new RefreshToken();
            if (_db.RefreshTokens.Any(r => r.DeviceId == userForAuthentication.UniqueId))
            {
                refreshToken = _db.RefreshTokens.First(r => r.DeviceId == userForAuthentication.UniqueId);
            }
            else
            {
                refreshToken.DeviceId = userForAuthentication.UniqueId;
                refreshToken.UserId = user.Id;
                refreshToken.Description = userForAuthentication.Description;
            }

            refreshToken.Token = _tokenService.GenerateRefreshToken();
            refreshToken.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            if (!_db.RefreshTokens.Any(r => r.DeviceId == userForAuthentication.UniqueId))
            {
                _db.RefreshTokens.Add(refreshToken);
            }

            _db.SaveChanges();

            var response = new AuthResponseDto();
            response.Token = token;
            response.UnId = user.Id;
            response.Successful = true;
            response.Error = string.Empty;
            response.IsAuthSuccessful = true;
            response.RefreshToken = refreshToken.Token;

            return Ok(response);
        }

        [HttpPost]
        [Route("refreshtoken")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto tokenDto)
        {
            if (tokenDto is null)
            {
                return BadRequest(new LoginResult { Successful = false, Error = "Invalid client request" });
            }

            var principal = Token.GetPrincipalFromExpiredToken(tokenDto.Token);

            string userName = principal.FindFirst("Name")?.Value;

            if (string.IsNullOrEmpty(userName))
            {
                userName = principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
            }

            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            var refreshToken = _db.RefreshTokens.First(r => r.UserId == user.Id && r.DeviceId == tokenDto.DeviceId);

            if (user == null || refreshToken.Token != tokenDto.RefreshToken || refreshToken.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest(new LoginResult { Successful = false, Error = "Invalid client request" });

            var token = Token.Generate(user.UserName, roles.ToList());

            refreshToken.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            _db.SaveChanges();

            return Ok(new LoginResult { Token = token, RefreshToken = refreshToken.Token, Successful = true });
        }

        [HttpPost]
        [Route("devicelogin")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(HostLogin hostLogin)
        {
            if (_db.Hosts.Any(h => h.Id == hostLogin.HostId))
            {
                var host = _db.Hosts.First(h => h.Id == hostLogin.HostId);

                if (host.Secret == hostLogin.HostSecret)
                {
                    var user = await _userManager.FindByIdAsync(host.ApplicationUserId.ToString());

                    var isValid = await _userManager.VerifyUserTokenAsync(user, "Default", "passwordless-auth", hostLogin.Token);

                    if (isValid)
                    {
                        await _userManager.UpdateSecurityStampAsync(user);

                        await HttpContext.SignInAsync(
                            IdentityConstants.ApplicationScheme,
                            new ClaimsPrincipal(
                                new ClaimsIdentity(
                                    new List<Claim>
                                    {
                            new Claim("sub", user.Id)
                                    },
                                    IdentityConstants.ApplicationScheme
                                )
                            )
                        );

                        var token = Token.Generate(user.UserName, new List<string>());
                        return Ok(token);
                    }
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes("401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1");
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private List<Claim> GetClaims(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: "staplereader.net",
                audience: "staplereader.net",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }
    }
}