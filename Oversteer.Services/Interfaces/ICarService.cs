using Oversteer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Services
{
    public interface ICarService
    {
        Task<List<CarClass>> GetCarClasses();
        Task<List<Car>> GetCarsInClass(Guid id);
    }
}
