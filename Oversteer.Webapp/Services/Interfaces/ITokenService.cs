using Microsoft.IdentityModel.Tokens;
using Oversteer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Oversteer.Webapp.Services
{
    public interface ITokenService
    {
        SigningCredentials GetSigningCredentials();
        Task<List<Claim>> GetClaims(ApplicationUser user);
        JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        ClaimsPrincipal GetPrincipalFromExpiredServerToken(string token, string key);
    }
}
