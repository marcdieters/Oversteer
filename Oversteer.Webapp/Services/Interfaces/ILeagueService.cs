using Oversteer.Models;

namespace Oversteer.Webapp.Services
{
    public interface ILeagueService
    {
        Task<List<League>> GetLeagues(LeagueOrder order);
        Task UpsertLeague(League league, byte[]? background, byte[]? logo);
        Task RemoveLeague(League league);
        Task<League> GetLeagueOfLoggedInUser();
    }
}
