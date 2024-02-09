using Newtonsoft.Json;
using Oversteer.Helpers;
using Oversteer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Services.Implementation
{
    public class RaceSimService : MustInitialize<string>, IRaceSimService
    {
        string _token;
        string _apiBaseAddress;

        public RaceSimService(string apiBaseAddress, string token) : base(token)
        {
            _token = token;
            _apiBaseAddress = apiBaseAddress;
        }

        public async Task<List<RaceSim>> GetAllRaceSims()
        {
            string output = await Api.Get(_apiBaseAddress, "/api/racesims", _token);
            if (!string.IsNullOrEmpty(output))
            {
                return JsonConvert.DeserializeObject<List<RaceSim>>(output);
            }
            else
            {
                return new List<RaceSim>();
            }
        }
    }
}
