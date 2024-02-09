using Newtonsoft.Json;
using Oversteer.Helpers;
using Oversteer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Services
{
    public class AuthenticationService : MustInitialize<string>, IAuthenticationService
    {
        string _apiBaseAddress;

        public AuthenticationService(string apiBaseAddress) : base(apiBaseAddress)
        {
            _apiBaseAddress = apiBaseAddress;
        }

        public async Task<RegisterResult> Register(RegisterDto registerDto)
        {
            RegisterResult registerResult = new RegisterResult();
            string result = await Api.Post(_apiBaseAddress, "api/account/register", registerDto, string.Empty);
            if (!string.IsNullOrEmpty(result))
            {
                registerResult = JsonConvert.DeserializeObject<RegisterResult>(result);
            }

            return registerResult;
        }

        public async Task<LoginResult> Login(LoginDto loginDto)
        {
            LoginResult loginResult = new LoginResult();
            string result = await Api.Post(_apiBaseAddress, "api/account/login", loginDto, string.Empty);
            if (!string.IsNullOrEmpty(result))
            {
                loginResult = JsonConvert.DeserializeObject<LoginResult>(result);
            }

            return loginResult;
        }

        public async Task<LoginResult> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            LoginResult loginResult = new LoginResult();
            string result = await Api.Post(_apiBaseAddress, "api/account/refreshtoken", refreshTokenDto, string.Empty);
            if (!string.IsNullOrEmpty(result))
            {
                loginResult = JsonConvert.DeserializeObject<LoginResult>(result);
            }

            return loginResult;
        }
    }
}
