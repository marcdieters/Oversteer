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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CountryController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CountryController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("countries")]
        public IActionResult GetCountries()
        {
            try
            {
                string userName = User.Claims.First(c => c.Type.Contains("name"))?.Value;
                List<Country> countries = _db.Countries.OrderBy(c => c.Name).ToList();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("country")]
        public IActionResult UpsertCountry([FromBody] Country country)
        {
            try
            {
                if (country.Id == Guid.Empty)
                {
                    _db.Countries.Add(country);
                }
                else
                {
                    _db.Entry(country).State = EntityState.Modified;
                }

                _db.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("country")]
        public IActionResult RemoveCountry(Guid id, Country country)
        {
            try
            {
                if (_db.Countries.Any(c => c.Id == id && country.Id == id))
                {
                    _db.Countries.Remove(country);
                    _db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
