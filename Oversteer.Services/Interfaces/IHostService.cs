using Oversteer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Services
{
    public interface IHostService
    {
        Task<string> GetHostKey();
        Task VerifyHost(string key);
        Task SaveHost(Host host);
        Task<List<Host>> GetHosts(Guid leagueId);
    }
}
