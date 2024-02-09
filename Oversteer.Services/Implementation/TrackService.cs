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
    public class TrackService : MustInitialize<string>, ITrackService
    {
        string _token;
        string _apiBaseAddress;

        public TrackService(string apiBaseAddress, string token) : base(token)
        {
            _token = token;
            _apiBaseAddress = apiBaseAddress;
        }

        public async Task<List<Track>> GetTracksInRaceSim(Guid racesimId)
        {
            string output = await Api.Get(_apiBaseAddress, $"/api/racesim/tracks/{racesimId}", _token);
            if (!string.IsNullOrEmpty(output))
            {
                return JsonConvert.DeserializeObject<List<Track>>(output);
            }
            else
            {
                return new List<Track>();
            }
        }
    }
}
