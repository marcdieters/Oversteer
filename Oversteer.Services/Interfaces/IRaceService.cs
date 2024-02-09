using Oversteer.Models.Racing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Services
{
    public interface IRaceService
    {
        Task<List<Race>> GetRaces();
        Task<List<Race>> GetUpcomingRaces();
        Task<Race> GetRace(Guid id);
        Task PostRace(Race race);
        Task PutRace(Guid id, Race race);
        Task DeleteRace(Guid id, Race race);
    }
}
