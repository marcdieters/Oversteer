using Newtonsoft.Json;
using Oversteer.Helpers;
using Oversteer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Services
{
    public class CarService : MustInitialize<string>, ICarService
    {
        string _token;
        string _apiBaseAddress;

        public CarService(string apiBaseAddress, string token) : base(token)
        {
            _token = token;
            _apiBaseAddress = apiBaseAddress;
        }

        public async Task<List<CarClass>> GetCarClasses()
        {
            string output = await Api.Get(_apiBaseAddress, "/api/car/classes", _token);
            if (!string.IsNullOrEmpty(output))
            {
                return JsonConvert.DeserializeObject<List<CarClass>>(output);
            }
            else
            {
                return new List<CarClass>();
            }
        }

        public async Task<List<Car>> GetCarsInClass(Guid id)
        {
            string output = await Api.Get(_apiBaseAddress, $"/api/car/inclass/{id}", _token);
            if (!string.IsNullOrEmpty(output))
            {
                return JsonConvert.DeserializeObject<List<Car>>(output);
            }
            else
            {
                return new List<Car>();
            }
        }
    }
}
