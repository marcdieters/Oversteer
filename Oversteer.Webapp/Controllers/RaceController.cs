using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oversteer.Models.Racing;
using Oversteer.Webapp.Data;

namespace Oversteer.Webapp.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RaceController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<RaceController> _logger;

        public RaceController(ApplicationDbContext db, ILogger<RaceController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        [Route("races")]
        public ActionResult GetRaces()
        {
            List<Race> races = _db.Races
                .Include(a => a.RaceSim)
                .Include(a => a.Track)
                .Include(a => a.Championship)
                .ToList();

            return Ok(races);
        }

        [HttpGet]
        [Route("races/upcoming")]
        public ActionResult GetUpcomingRaces()
        {
            List<Race> races = _db.Races
                .Include(a => a.Sessions)
                .Where(a => a.Complete == false && a.Scheduled == false && a.StartTime > DateTime.UtcNow)
                .ToList();

            return Ok(races);
        }

        [HttpGet]
        [Route("race/{id:guid}")]
        public ActionResult GetRace(Guid id)
        {
            if (_db.Races.Any(a => a.Id == id))
            {
                Race race = _db.Races
                .Include(a => a.RaceSim)
                .Include(a => a.Track)
                .Include(a => a.Championship)
                .Include(a => a.Acc)
                .Include(a => a.Sessions)
                .Include(a => a.League)
                .ThenInclude(b => b.Hosts)
                .First(a => a.Id == id);

                return Ok(race);
            }
            else
            {
                _logger.LogWarning($"Race with id {id} not found");
                return NotFound();
            }
        }

        [HttpPost]
        [Route("race")]
        public async Task<ActionResult> PostRace(Race race)
        {
            if (!_db.Races.Any(a => a.Id == race.Id))
            {
                await _db.Races.AddAsync(race);
                await _db.SaveChangesAsync();
                return Ok();
            }
            else
            {
                _db.Entry(race).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpDelete]
        [Route("race/{id:guid}")]
        public async Task<ActionResult> DeleteRace(Guid id, [FromBody] Race race)
        {
            if (_db.Races.Any(a => a.Id == race.Id && race.Id == id))
            {
                _db.Races.Remove(race);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                _logger.LogWarning($"Race with id {id} not found");
                return NotFound();
            };
        }
    }
}
