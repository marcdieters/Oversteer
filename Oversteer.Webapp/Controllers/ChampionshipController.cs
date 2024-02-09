using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using Oversteer.Models;
using Oversteer.Models.Racing;
using Oversteer.Webapp.Data;
using Championship = Oversteer.Models.Racing.Championship;

namespace Oversteer.Webapp.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChampionshipController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ChampionshipController> _logger;

        public ChampionshipController(ApplicationDbContext db, ILogger<ChampionshipController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        [Route("championships")]
        public ActionResult GetChampionships()
        {
            List<Championship> championships = _db.Championships
                .Include(a => a.CarClasses)
                .Include(a => a.Points)
                .Include(a => a.Cars)
                .Include(a => a.Races)
                .Include(a => a.RaceSim)
                .ToList();

            return Ok(championships);
        }

        [HttpGet]
        [Route("championship/{id:guid}")]
        public ActionResult GetChampionship(Guid id)
        {
            if (_db.Championships.Any(a => a.Id == id))
            {
                Championship championship = _db.Championships
                    .Include(a => a.CarClasses)
                    .Include(a => a.Points)
                    .Include(a => a.Cars)
                    .Include(a => a.Splits)
                    .Include(a => a.Points)
                    .Include(a => a.ACC)
                    .Include(a => a.Races)
                    .ThenInclude(b => b.Track)
                    .First(a => a.Id == id);

                return Ok(championship);
            }
            else
            {
                _logger.LogWarning($"Championship with id {id} not found");
                return NotFound();
            }
        }

        [HttpPost]
        [Route("championship")]
        public async Task<ActionResult> PostChampionship(Championship championship)
        {
            if (!_db.Championships.Any(a => a.Id == championship.Id))
            {
                await _db.Championships.AddAsync(championship);
                await _db.SaveChangesAsync();
            }
            else
            {
                _db.Championships.Update(championship);
                await _db.SaveChangesAsync();
                return NoContent();
            }

            return Ok();
        }

        [HttpDelete]
        [Route("championship/{id:guid}")]
        public async Task<ActionResult> DeleteChampionship(Guid id, [FromBody] Championship championship)
        {
            if (_db.Championships.Any(a => a.Id == championship.Id && championship.Id == id))
            {
                _db.Championships.Remove(championship);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                _logger.LogWarning($"Championship with id {id} not found");
                return NotFound();
            };
        }
    }
}
