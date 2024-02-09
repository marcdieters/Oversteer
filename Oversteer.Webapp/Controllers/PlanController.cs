using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oversteer.Models;
using Oversteer.Webapp.Data;
using System.Data;

namespace Oversteer.Webapp.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlanController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public PlanController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("plans")]
        public IActionResult GetAllPlans()
        {
            try
            {
                List<Plan> plans = _db.Plans.Include(p => p.Features).OrderBy(c => c.Name).ToList();
                return Ok(plans);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
