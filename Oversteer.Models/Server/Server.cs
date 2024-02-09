using Oversteer.Models.Racing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oversteer.Enums;

namespace Oversteer.Models
{
    public class Server
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public Guid RaceSimId { get; set; }
        public RaceSim? RaceSim { get; set; }
        public Guid CarClassId { get; set; }
        public CarClass? CarClass { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime StartedWhen { get; set; }
        public Guid HostId { get; set; }
        public Host? Host { get; set; }
        public int UDPPort { get; set; }
        public int TCPPort { get; set; }
        public int HTTPPort { get; set; }
        public string Password { get; set; } = string.Empty;
        public string AdminPassword { get; set; } = string.Empty;
        public string FilesLocation { get; set; } = string.Empty;
        public int Version { get; set; }
        public Guid TrackId { get; set; }
        public Track? Track { get; set; }
        public Guid? TrackLayoutId { get; set; }
        public TrackLayout? TrackLayout { get; set; }
        public string CurrentSession { get; set; } = string.Empty;
        public int ProcessId { get; set; }
        public Guid LeagueId { get; set; }
        public League? League { get; set; }
        public Guid RaceId { get; set; }
        public Race? Race { get; set; }
        public bool PreQualyRulesApply { get; set; }
        public bool PracticeServer { get; set; }
        public List<Session> Sessions { get; set; } = new List<Session>();
        public WeatherType WeatherType { get; set; }
        public ACC ACC { get; set; } = new ACC();
    }
}
