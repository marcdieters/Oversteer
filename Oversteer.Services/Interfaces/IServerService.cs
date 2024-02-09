using Oversteer.Models;
using Oversteer.Models.Racing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Services
{
    public interface IServerService
    {
        Task<List<Server>> GetServers();
        Task<Server> GetServer(Guid id);
        Task PostServer(Server race);
        Task DeleteServer(Guid id, Server race);
    }
}
