namespace Oversteer.Webapp.Services
{
    public interface IAccountService
    {
        Task<string> GetClaimValue(string type);
        Task<List<string>> GetRoles();
        Task<string> GenerateToken();
    }
}
