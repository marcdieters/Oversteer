using Newtonsoft.Json;
using Oversteer.Helpers;
using Oversteer.Models.Racing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Services
{
    public class ChampionshipService : MustInitialize<string>, IChampionshipService
    {
        string _token;
        string _apiBaseAddress;

        public ChampionshipService(string apiBaseAddress, string token) : base(token)
        {
            _token = token;
            _apiBaseAddress = apiBaseAddress;
        }

        public async Task<List<Championship>> GetChampionships()
        {
            string result = await Api.Get(_apiBaseAddress, "api/championships", _token);
            List<Championship> championships = JsonConvert.DeserializeObject<List<Championship>>(result);
            return championships;
        }

        public async Task<Championship> GetChampionship(Guid id)
        {
            string result = await Api.Get(_apiBaseAddress, $"api/championship/{id}", _token);
            Championship championship = JsonConvert.DeserializeObject<Championship>(result);
            return championship;
        }

        public async Task PostChampionship(Championship championship)
        {
            await Api.Post(_apiBaseAddress, "api/championship", championship, _token);
        }

        public async Task PutChampionship(Guid id, Championship championship)
        {
            await Api.Put(_apiBaseAddress, $"api/championship/{id}", championship, _token);
        }

        public async Task DeleteChampionship(Guid id, Championship championship)
        {
            await Api.Delete(_apiBaseAddress, $"api/championship/{id}", championship, _token);
        }
    }
}
