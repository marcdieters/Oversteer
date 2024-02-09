using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Oversteer.Models;
using Oversteer.Webapp.Data;
using System.Diagnostics.Metrics;
using System.Formats.Asn1;

namespace Oversteer.Webapp.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly IImageService _imageService;
        private readonly ApplicationDbContext _db;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public LeagueService(IImageService imageService, ApplicationDbContext db, AuthenticationStateProvider authenticationStateProvider)
        {
            _imageService = imageService;
            _db = db;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public Task<List<League>> GetLeagues(LeagueOrder order)
        {
            var leagues = _db.Leagues.Where(l => l.Approved == true).ToList();

            switch (order)
            {
                case LeagueOrder.Name:
                    leagues = leagues.OrderBy(l => l.Name).ToList();
                    break;

                case LeagueOrder.LastUpdate:
                    leagues = leagues.OrderBy(l => l.Updated).ToList();
                    break;
            }

            return Task.FromResult(leagues);
        }

        public async Task RemoveLeague(League league)
        {
            if (_db.Leagues.Any(c => c.Id == league.Id))
            {
                _db.Leagues.Remove(league);
                _db.SaveChanges();

                if (!string.IsNullOrEmpty(league.Logo))
                    await _imageService.RemoveImage("img", league.Logo);

                if (!string.IsNullOrEmpty(league.BackGrounndImage))
                    await _imageService.RemoveImage("img", league.BackGrounndImage);
            }
        }

        public async Task UpsertLeague(League league, byte[]? background, byte[]? logo)
        {
            if (background != null)
                await _imageService.SaveImage(background, Path.Combine("img", league.BackGrounndImage));

            if (logo != null)
                await _imageService.SaveImage(logo, Path.Combine("img", league.Logo));

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            league.OwnerId = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (!_db.Leagues.Any(c => c.Id == league.Id))
            {
                _db.Leagues.Add(league);
            }
            else
            {
                _db.Entry(league).State = EntityState.Modified;
            }

            _db.SaveChanges();
        }

        public async Task<League> GetLeagueOfLoggedInUser()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userId = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (_db.Leagues.Any(l => l.OwnerId == userId))
            {
                var league = _db.Leagues.First(l => l.OwnerId == userId);
                return league;
            }

            return null;
        }
    }
}
