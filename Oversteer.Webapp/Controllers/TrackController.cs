using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oversteer.Models;
using Oversteer.Webapp.Data;

namespace Oversteer.Webapp.Controllers
{
    [Route("api")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public TrackController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("tracks")]
        public IActionResult GetTracks()
        {
            try
            {
                List<Track> tracks = _db.Tracks
                    .OrderBy(c => c.Name)
                    .ToList();

                return Ok(tracks);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("racesim/tracks/{racesimid:guid}")]
        public IActionResult GetTracksInRaceSim(Guid racesimId)
        {
            try
            {
                List<Track> tracks = _db.TrackInRaceSims
                    .Include(t => t.Track)
                    .ThenInclude(t => t.TrackLayouts)
                    .Where(t => t.RaceSimId ==  racesimId)
                    .Select(t => t.Track)
                    .OrderBy(c => c.Name)
                    .ToList();

                return Ok(tracks);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
