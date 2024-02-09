using Cronos;
using Hangfire.Common;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Oversteer.Helpers;
using Oversteer.Models.Racing;
using Oversteer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oversteer.Models;
using System.Runtime.InteropServices;

namespace Oversteer.Scheduler
{
    public class WorkerService : IHostedService, IDisposable
    {
        private readonly IConfiguration _configuration;
        private IRaceService RaceService { get; set; }
        private IServerService ServerService { get; set; }

        public WorkerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Dispose()
        {

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string token = Token.Generate("scheduler", new List<string>());
            RaceService = new RaceService(_configuration["OversteerBaseUri"], token);
            ServerService = new ServerService(_configuration["OversteerBaseUri"], token);

            List<Race> races = await RaceService.GetUpcomingRaces();
            foreach (Race race in races)
            {
                DateTimeOffset offset = new DateTimeOffset(race.StartTime);
                BackgroundJob.Schedule(() => StartServer(race.Id), offset);

                race.Scheduled = true;
                await RaceService.PostRace(race);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }

        public async void StartServer(Guid raceId)
        {
            Race race = await RaceService.GetRace(raceId);
            Server server = new Server();
            server.ACC = race.Acc;

            server.AdminPassword = string.Empty;
            server.CarClass = race.Splits.First().CarClass;
            server.CarClassId = server.CarClass.Id;
            server.LeagueId = Guid.Parse(race.LeagueId.ToString());
            server.Name = race.LobbyName;
            server.Sessions = race.Sessions;
            server.Password = race.Password;
            server.Track = race.Track;
            server.TrackId = race.TrackId;
            server.HostId = race.League.Hosts.First().Id;

            await ServerService.PostServer(server);
        }
    }
}
