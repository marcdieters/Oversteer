using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Services
{
    public interface IRaceSimService
    {
        Task<List<Models.RaceSim>> GetAllRaceSims();
    }
}
