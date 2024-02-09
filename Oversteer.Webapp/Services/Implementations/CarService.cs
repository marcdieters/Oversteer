using Newtonsoft.Json;
using Oversteer.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System;
using System.Text;
using Oversteer.Webapp.Data;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;

namespace Oversteer.Webapp.Services
{
    public class CarService : ICarService
    {
        private readonly IImageService _imageService;
        private readonly ApplicationDbContext _db;

        public CarService(IImageService imageService, ApplicationDbContext db)
        {
            _imageService = imageService;
            _db = db;
        }

        public Task<List<CarClass>> GetCarClasses()
        {
            var carclasses = _db.CarClasses.OrderBy(c => c.Name).ToList();
            return Task.FromResult(carclasses);
        }

        public async Task RemoveCarClass(CarClass carClass)
        {
            if (_db.CarClasses.Any(c => c.Id == carClass.Id))
            {
                _db.CarClasses.Remove(carClass);
                _db.SaveChanges();

                await _imageService.RemoveImage("img", carClass.Logo);
            }
        }

        public async Task UpsertCarClass(CarClass carClass, byte[] fileContent)
        {
            await _imageService.SaveImage(fileContent, Path.Combine("img", carClass.Logo));
            if (!_db.CarClasses.Any(c => c.Id == carClass.Id))
            {
                _db.CarClasses.Add(carClass);
            }
            else
            {
                _db.Entry(carClass).State = EntityState.Modified;
            }

            _db.SaveChanges();
        }


        public Task<List<CarBrand>> GetCarBrands()
        {
            var carbrands = _db.CarBrands.Include(c => c.Country).OrderBy(c => c.Name).ToList();
            return Task.FromResult(carbrands);

        }

        public async Task UpsertCarBrand(CarBrand carBrand, byte[] fileContent)
        {
            await _imageService.SaveImage(fileContent, Path.Combine("img", carBrand.Logo));

            if (!_db.CarBrands.Any(c => c.Id == carBrand.Id))
            {
                _db.CarBrands.Add(carBrand);
            }
            else
            {
                _db.Entry(carBrand).State = EntityState.Modified;
            }

            _db.SaveChanges();
        }

        public async Task RemoveCarBrand(CarBrand carBrand)
        {
            if (_db.CarBrands.Any(c => c.Id == carBrand.Id))
            {
                _db.CarBrands.Remove(carBrand);
                _db.SaveChanges();

                await _imageService.RemoveImage("img", carBrand.Logo);
            }
        }

        public Task<List<Car>> GetCars()
        {
            var cars = _db.Cars.Include(c => c.CarBrand).OrderBy(c => c.Name).ToList();
            return Task.FromResult(cars);
        }

        public async Task<Guid> UpsertCar(Car car, byte[] fileContent)
        {
            await _imageService.SaveImage(fileContent, Path.Combine("img", car.Image));

            if (!_db.Cars.Any(c => c.Id == car.Id))
            {
                _db.Cars.Add(car);
            }
            else
            {
                _db.Entry(car).State = EntityState.Modified;
            }

            _db.SaveChanges();

            return car.Id;
        }

        public async Task RemoveCar(Car car)
        {
            if (_db.Cars.Any(c => c.Id == car.Id))
            {
                _db.Cars.Remove(car);
                _db.SaveChanges();

                await _imageService.RemoveImage("img", car.Image);
            }
        }
    }
}
