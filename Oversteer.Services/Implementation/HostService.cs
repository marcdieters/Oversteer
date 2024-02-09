using Newtonsoft.Json;
using Oversteer.Helpers;
using Oversteer.Models;
using Oversteer.Models.Racing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Services
{
    public class HostService : MustInitialize<string>, IHostService
    {
        string _token;
        string _apiBaseAddress;

        public HostService(string apiBaseAddress, string token) : base(token)
        {
            _token = token;
            _apiBaseAddress = apiBaseAddress;
        }

        public async Task<string> GetHostKey()
        {
            string key = await Api.Get(_apiBaseAddress, "/api/host/key", _token);
            return key;
        }

        public async Task VerifyHost(string key)
        {
            await Api.Post(_apiBaseAddress, "api/host/verify", key, _token, false);
        }

        public async Task<List<Host>> GetHosts(Guid leagueId)
        {
            string output = await Api.Get(_apiBaseAddress, $"/api/hosts/{leagueId}", _token);
            if (!string.IsNullOrEmpty(output))
            {
                return JsonConvert.DeserializeObject<List<Host>>(output);
            }
            else
            {
                return new List<Host>();
            }
        }

        public async Task SaveHost(Host host)
        {
            await Api.Post(_apiBaseAddress, "api/host", host, _token);
        }
    }
}
