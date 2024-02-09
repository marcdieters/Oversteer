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
    public class ServerService : MustInitialize<string>, IServerService
    {
        string _token;
        string _apiBaseAddress;

        public ServerService(string apiBaseAddress, string token) : base(token)
        {
            _token = token;
            _apiBaseAddress = apiBaseAddress;
        }

        public async Task<List<Server>> GetServers()
        {
            string result = await Api.Get(_apiBaseAddress, "api/servers", _token);
            List<Server> servers = JsonConvert.DeserializeObject<List<Server>>(result);
            return servers;
        }

        public async Task<Server> GetServer(Guid id)
        {
            string result = await Api.Get(_apiBaseAddress, $"api/server/{id}", _token);
            Server server = JsonConvert.DeserializeObject<Server>(result);
            return server;
        }

        public async Task PostServer(Server server)
        {
            await Api.Post(_apiBaseAddress, "api/server", server, _token);
        }

        public async Task DeleteServer(Guid id, Server server)
        {
            await Api.Delete(_apiBaseAddress, $"api/server/{id}", server, _token);
        }
    }
}
