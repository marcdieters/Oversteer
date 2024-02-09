using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oversteer.Enums;
using Oversteer.Models;
using Oversteer.Models.Racing;
using Oversteer.Models.Result;
using Oversteer.Webapp.Data;
using static NuGet.Packaging.PackagingConstants;

namespace Oversteer.Webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public UploadController(ApplicationDbContext db, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("acc/{raceid:guid}")]
        public async Task<ActionResult> UploadAcc(Guid raceId, [FromBody] ResultDto resultDto)
        {
            if (raceId == resultDto.RaceId && _db.Races.Any(r => r.Id == raceId))
            {
                if (_db.Results.Any(r => r.RaceId ==  raceId && r.SessionType == resultDto.SessionType))
                {
                    var resultFromDb = _db.Results.Include(r => r.Leaderboard).Include(r => r.Laps).First(r => r.RaceId == raceId);
                    _db.Results.Remove(resultFromDb);
                    await _db.SaveChangesAsync();
                }

                List<CarClass> carClasses = _db.CarClasses.ToList();

                Result result = new Result();
                result = _mapper.Map<Result>(resultDto);
                result.IsWet = Convert.ToBoolean(resultDto.ACCResult.sessionResult.isWetSession);

                int postition = 1;
                foreach(var accLeaderboard in resultDto.ACCResult.sessionResult.leaderBoardLines)
                {
                    Leaderboard leaderboard = new Leaderboard();
                    leaderboard.ResultId = result.Id;
                    leaderboard.Position = postition;
                    leaderboard.RaceNumber = accLeaderboard.car.raceNumber;
                    leaderboard.CupCategory = (CupCategory)accLeaderboard.car.cupCategory;
                    leaderboard.DriverTotalTimes = accLeaderboard.driverTotalTimes.Sum(d => d);
                    leaderboard.TotalLaps = accLeaderboard.timing.lapCount;
                    leaderboard.BestLap = accLeaderboard.timing.bestLap;
                    leaderboard.LastLap = accLeaderboard.timing.lastLap;
                    leaderboard.InGameCarId = accLeaderboard.car.carId;

                    if (carClasses.Any(c => c.Name == accLeaderboard.car.carGroup))
                    {
                        leaderboard.CarclassId = carClasses.First(c => c.Name == accLeaderboard.car.carGroup).Id;
                    }

                    int driverIndex = 0;
                    foreach(var accDriver in accLeaderboard.car.drivers)
                    {
                        Driver driver = new Driver();

                        if (_db.Drivers.Any(d => d.SteamId == accDriver.playerId))
                        {
                            driver = _db.Drivers.First(d => d.SteamId == accDriver.playerId);
                        }
                        else
                        {
                            driver = new Driver();
                            driver.FirstName = accDriver.firstName;
                            driver.LastName = accDriver.lastName;
                            driver.SteamId = accDriver.playerId;

                            if (_db.Users.Any(u => u.SteamId == accDriver.playerId))
                            {
                                ApplicationUser user = _db.Users.First(u => u.SteamId == accDriver.playerId);
                                driver.UserId = user.Id;
                            }

                            await _db.Drivers.AddAsync(driver);
                            await _db.SaveChangesAsync();
                        }

                        leaderboard.DriverId = driver.Id;
                        leaderboard.DriverIndex = driverIndex;

                        driverIndex += 1;
                    }

                    result.Leaderboard.Add(leaderboard);
                    postition += 1;
                }

                foreach(var acclap in resultDto.ACCResult.laps)
                {
                    Lap lap = new Lap();
                    lap.ResultId = result.Id;
                    lap.DriverId = result.Leaderboard.First(l => l.InGameCarId == acclap.carId && l.DriverIndex == acclap.driverIndex).Id;
                    lap.LapTime = acclap.laptime;
                    lap.IsValidForBest = acclap.isValidForBest;
                    result.Laps.Add(lap);
                }

                result.TimeStamp = DateTime.Now;
                result.LogFileRaw = JsonConvert.SerializeObject(resultDto.ACCResult);
                await _db.Results.AddAsync(result);
                await _db.SaveChangesAsync();

                Race race = _db.Races.First(r => r.Id == raceId);
                race.Complete = true;
                race.CompleteTime = DateTime.Now;
                await _db.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
