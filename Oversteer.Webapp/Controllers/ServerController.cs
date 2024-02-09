using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oversteer.Models;
using Oversteer.Services;
using Oversteer.Webapp.Data;
using Oversteer.Webapp.Services;
using System.Globalization;
using System.Text.RegularExpressions;
using Host = Oversteer.Models.Host;

namespace Oversteer.Webapp.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ServerController : ControllerBase
    {
        private readonly IMessageProducer _messagePublisher;
        private readonly ApplicationDbContext _db;
        private UserManager<ApplicationUser> _userManager;

        public ServerController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IMessageProducer messagePublisher)
        {
            _db = db;
            _userManager = userManager;
            _messagePublisher = messagePublisher;
        }

        [HttpGet]
        [Route("server/download/{racesimId:guid}")]
        public IActionResult Download(Guid racesimId)
        {
            RaceSim raceSim = _db.RaceSims.First(s => s.Id == racesimId);
            string fileToSent = $"{raceSim.FilesLocation}_{raceSim.Version}.zip"; 

            byte[] fileInBytes = System.IO.File.ReadAllBytes(Path.Combine("Servers", fileToSent));

            MemoryStream stream = new MemoryStream(fileInBytes);

            if (stream == null)
                return NotFound(); // returns a NotFoundResult with Status404NotFound response.

            return File(stream, "application/octet-stream"); // returns a FileStreamResult
        }

        [HttpGet]
        [Route("servers")]
        public ActionResult GetServers()
        {
            try
            {
                List<Server> servers = _db.Servers
                    .Include(s => s.RaceSim)
                    .Include(s => s.Host)
                    .OrderBy(c => c.Name)
                    .ToList();

                return Ok(servers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("server/{serverid:guid}")]
        public async Task<ActionResult> GetServer(Guid serverId)
        {
            try
            {
                if (_db.Servers.Any(s => s.Id == serverId))
                {
                    Server server = await _db.Servers
                        .Include(s => s.RaceSim)
                        .Include(s => s.Host)
                        .FirstAsync(s => s.Id == serverId);

                    return Ok(server);
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

        [HttpPost]
        [Route("server")]
        public async Task<ActionResult> Post([FromBody] Server server)
        {
            Host hostToUse = _db.Hosts.AsNoTracking().First(h => h.Id == server.HostId);
            List<Server> servers = _db.Servers.AsNoTracking().Where(s => s.HostId == server.HostId).ToList();
            
            int tcpPort = hostToUse.TcpStartPort;
            int udpPort = hostToUse.UdpStartPort;

            foreach (var serverInDb in servers)
            {
                if (serverInDb.TCPPort == tcpPort)
                {
                    tcpPort += 1;
                }

                if (serverInDb.UDPPort == udpPort)
                {
                    udpPort += 1;
                }
            }

            server.TCPPort = tcpPort;
            server.UDPPort = udpPort;

            if (_db.Servers.Any(h => h.Id == server.Id))
            {
                _db.Entry(server).State = EntityState.Modified;
            }
            else
            {
                await _db.Servers.AddAsync(server);
            }

            await _db.SaveChangesAsync();

            // Sent message to host
            server.RaceSim = _db.RaceSims.AsNoTracking().First(r => r.Id == server.RaceSimId);
            server.CarClass = _db.CarClasses.AsNoTracking().First(c => c.Id == server.CarClassId);
            server.Track = _db.Tracks.AsNoTracking().Include(t => t.TrackInRaceSims).First(t => t.Id == server.TrackId);

            HostEventMessage hostEventMessage = new HostEventMessage();
            hostEventMessage.HostEventType = HostEventType.CreateServer;
            hostEventMessage.Body = JsonConvert.SerializeObject(server, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
            
            _messagePublisher.SendMessage(hostEventMessage, hostToUse.QueueName);

            return Ok();
        }

        [HttpDelete]
        [Route("server")]
        public async Task<ActionResult> RemoveServer(Guid id, Server server)
        {
            try
            {
                if (_db.Servers.Any(c => c.Id == id && server.Id == id))
                {
                    _db.Servers.Remove(server);
                    await _db.SaveChangesAsync();
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
