using Oversteer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Services
{
    public interface IAuthenticationService
    {
        Task<RegisterResult> Register(RegisterDto registerDto);
        Task<LoginResult> Login(LoginDto loginDto);
        Task<LoginResult> RefreshToken(RefreshTokenDto refreshTokenDto);
    }
}
