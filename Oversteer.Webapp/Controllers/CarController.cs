using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oversteer.Models;
using Oversteer.Webapp.Data;

namespace Oversteer.Webapp.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admins")]
    public class CarController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CarController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("car/classes")]
        public IActionResult GetAllCarClasses()
        {
            try
            {
                List<CarClass> classes = _db.CarClasses
                    .OrderBy(c => c.Name)
                    .ToList();
                
                return Ok(classes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("car/inclass/{carclassid:guid}")]
        public IActionResult GetCars(Guid carClassId)
        {
            try
            {
                List<Car> cars = _db.Cars
                    .Where(c => c.CarClassId == carClassId)
                    .OrderBy(c => c.Name)
                    .ToList();

                return Ok(cars);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
