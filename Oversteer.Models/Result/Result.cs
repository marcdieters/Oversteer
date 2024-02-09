using Oversteer.Models.Racing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oversteer.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oversteer.Models.Result
{
    public class Result
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public SessionType SessionType { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid RaceId { get; set; }
        public Race? Race { get; set; }
        public string FileName { get; set; } = string.Empty;
        public bool IsWet { get; set; }
        public string LogFileRaw { get; set; } = string.Empty;
        public List<Leaderboard> Leaderboard { get; set; } = new List<Leaderboard>();
        public List<Lap> Laps { get; set; } = new List<Lap>();
    }

    public class Leaderboard
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ResultId { get; set; }
        public Result? Result { get; set; }
        public Guid DriverId { get; set; }
        public Driver? Driver { get; set; }
        public int DriverIndex { get; set; }
        public int RaceNumber { get; set; }
        public int InGameCarId { get; set; }
        public double DriverTotalTimes { get; set; }
        public int Position { get; set; }
        public int TotalLaps { get; set; }
        public CupCategory CupCategory { get; set; }
        public Guid CarclassId { get; set; }
        public CarClass? CarClass { get; set; }
        public double BestLap { get; set; }
        public double LastLap { get; set; }
    }

    public class Lap
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ResultId { get; set; }
        public Result? Result { get; set; }
        public Guid DriverId { get; set; }
        public Driver? Driver { get; set; }
        public double LapTime { get; set; }
        public bool IsValidForBest { get; set; }
        public int DriverIndex { get; set; }
    }
}
