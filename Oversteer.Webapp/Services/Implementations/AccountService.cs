using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Xml.Linq;

namespace Oversteer.Webapp.Services
{
    public class AccountService : IAccountService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AccountService(AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorage)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<string> GenerateToken()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            var id = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            var name = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
            var roleClaims = authState.User.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").ToList();

            var roles = await GetRoles();

            bool isTokenValid = false;
            string currentToken = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(currentToken))
            {
                isTokenValid = Helpers.Token.IsValid(currentToken);
            }

            if (isTokenValid)
            {
                return currentToken;
            }
            else
            {
                string token = Helpers.Token.Generate(name, roles);
                await _localStorage.SetItemAsync<string>("authToken", token);

                return token;
            }
        }

        public async Task<string> GetClaimValue(string type)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            var value = authState.User.Claims.FirstOrDefault(c => c.Type.ToLower().Contains(type)).Value;

            return value;
        }

        public async Task<List<string>> GetRoles()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            var roleClaims = authState.User.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").ToList();

            List<string> roles = new List<string>();
            foreach (var role in roleClaims)
            {
                roles.Add(role.Value);
            }

            return roles;
        }
    }
}
