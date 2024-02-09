using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oversteer.Models;
using Oversteer.Webapp.Data;

namespace Oversteer.Webapp.Controllers
{
    [Route("api")]
    [ApiController]
    public class RacesimController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public RacesimController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("racesims")]
        public ActionResult GetRacesims()
        {
            try
            {
                List<RaceSim> racesims = _db.RaceSims.OrderBy(c => c.Name).ToList();
                return Ok(racesims);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
