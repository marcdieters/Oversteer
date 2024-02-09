using Oversteer.Models.Racing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Services
{
    public interface IChampionshipService
    {
        Task<List<Championship>> GetChampionships();
        Task<Championship> GetChampionship(Guid id);
        Task PostChampionship(Championship championship);
        Task PutChampionship(Guid id, Championship championship);
        Task DeleteChampionship(Guid id, Championship championship);
    }
}
