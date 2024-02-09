using Oversteer.Models;

namespace Oversteer.Webapp.Services
{
    public interface ICarService
    {
        Task<List<CarClass>> GetCarClasses();
        Task UpsertCarClass(CarClass carClass, byte[] fileContent);
        Task RemoveCarClass(CarClass carClass);

        Task<List<CarBrand>> GetCarBrands();
        Task UpsertCarBrand(CarBrand carClass, byte[] fileContent);
        Task RemoveCarBrand(CarBrand carClass);

        Task<List<Car>> GetCars();
        Task<Guid> UpsertCar(Car car, byte[] fileContent);
        Task RemoveCar(Car car);

    }
}
