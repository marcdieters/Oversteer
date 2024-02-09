using Newtonsoft.Json;
using Oversteer.Helpers;
using Oversteer.Models.Racing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Services
{
    public class RaceService : MustInitialize<string>, IRaceService
    {
        string _token;
        string _apiBaseAddress;

        public RaceService(string apiBaseAddress, string token) : base(token)
        {
            _token = token;
            _apiBaseAddress = apiBaseAddress;
        }

        public async Task<List<Race>> GetRaces()
        {
            string result = await Api.Get(_apiBaseAddress, "api/races", _token);
            List<Race> races = JsonConvert.DeserializeObject<List<Race>>(result);
            return races;
        }

        public async Task<List<Race>> GetUpcomingRaces()
        {
            string result = await Api.Get(_apiBaseAddress, "api/races/upcoming", _token);
            List<Race> races = JsonConvert.DeserializeObject<List<Race>>(result);
            return races;
        }

        public async Task<Race> GetRace(Guid id)
        {
            string result = await Api.Get(_apiBaseAddress, $"api/race/{id}", _token);
            Race race = JsonConvert.DeserializeObject<Race>(result);
            return race;
        }

        public async Task PostRace(Race race)
        {
            await Api.Post(_apiBaseAddress, "api/race", race, _token);
        }

        public async Task PutRace(Guid id, Race race)
        {
            await Api.Put(_apiBaseAddress, $"api/race/{id}", race, _token);
        }

        public async Task DeleteRace(Guid id, Race race)
        {
            await Api.Delete(_apiBaseAddress, $"api/race/{id}", race, _token);
        }
    }
}
