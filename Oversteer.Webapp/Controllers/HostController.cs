using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using Oversteer.Extensions;
using Oversteer.Helpers;
using Oversteer.Models;
using Oversteer.Webapp.Data;
using Oversteer.Webapp.Services;
using System.Diagnostics.Metrics;
using Host = Oversteer.Models.Host;

namespace Oversteer.Webapp.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HostController : ControllerBase
    {
        private readonly IMessageProducer _messagePublisher;
        private readonly ApplicationDbContext _db;
        private UserManager<ApplicationUser> _userManager;

        public HostController(IMessageProducer messagePublisher, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _messagePublisher = messagePublisher;
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("hosts/{league:guid}")]
        public IActionResult GetHosts(Guid leagueId)
        {
            List<Host> hosts = new List<Host>();

            if (leagueId == Guid.Empty)
            {
                hosts = _db.Hosts.OrderBy(a => a.Name).ToList();
            }
            else
            {
                hosts = _db.Hosts.Where(a => a.LeagueId == leagueId).OrderBy(a => a.Name).ToList();
            }

            return Ok(hosts);
        }

        [HttpGet]
        [Route("host/key")]
        public async Task<ActionResult> GetHostKey()
        {
            string userName = User.Claims.First(c => c.Type.Contains("name"))?.Value;
            ApplicationUser user = await _userManager.FindByNameAsync(userName);
            user.ServerAuthKey = Cryptography.GenerateRandomPassword();
            user.ServerKeyValidUntil = DateTime.UtcNow.AddMinutes(30);
            await _userManager.UpdateAsync(user);

            return Ok(user.ServerAuthKey);
        }

        [HttpPost]
        [Route("host/verify")]
        public async Task<ActionResult> Verify([FromBody] string serverAuthKey)
        {
            if (_db.Users.Any(u => u.ServerAuthKey == serverAuthKey))
            {
                string email = _db.Users.First(u => u.ServerAuthKey == serverAuthKey).Email;
                ApplicationUser user = await _userManager.FindByEmailAsync(email);

                string userKey = user.ServerAuthKey.DeepCopy();

                user.ServerAuthKey = string.Empty;
                await _userManager.UpdateAsync(user);

                if (userKey == serverAuthKey && user.ServerKeyValidUntil > DateTime.UtcNow)
                {
                    return Ok(user.Id);
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        [Route("host")]
        public async Task<ActionResult> Post([FromBody] Host host)
        {
            if (_db.Hosts.Any(h => h.Id == host.Id))
            {
                _db.Entry(host).State = EntityState.Modified;
            }
            else
            {
                await _db.Hosts.AddAsync(host);
            }

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("host/preauth")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(HostPreAuthDto model)
        {
            // Login...
            if (_db.Hosts.Any(h => h.Id == model.HostId && h.Secret == model.HostSecret))
            {
                var host = _db.Hosts.First(h => h.Id == model.HostId);
                var user = await _userManager.FindByIdAsync(host.ApplicationUserId.ToString());

                var returnUrl = HttpContext?.Request.Query.FirstOrDefault(r => r.Key == "returnUrl");

                var token = await _userManager.GenerateUserTokenAsync(user, "Default", "passwordless-auth");

                HostPreAuthResponse hostPreAuthResponse = new HostPreAuthResponse();

                hostPreAuthResponse.Token = token;
                hostPreAuthResponse.HostId = model.HostId;

                return Ok(hostPreAuthResponse);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
