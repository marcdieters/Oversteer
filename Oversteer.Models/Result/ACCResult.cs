using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Models.Result
{
    public class ACCResult
    {
        public string? sessionType { get; set; }
        public string? trackName { get; set; }
        public int sessionIndex { get; set; }
        public int raceWeekendIndex { get; set; }
        public string? metaData { get; set; }
        public string? serverName { get; set; }
        public int sessionResultId { get; set; }
        public ACCSessionResult sessionResult { get; set; } = new ACCSessionResult();
        public List<ACCLap> laps { get; set; } = new List<ACCLap>();
        public List<ACCPenalty> penalties { get; set; }= new List<ACCPenalty>();
        public List<ACCPenalty> post_race_penalties { get; set; } = new List<ACCPenalty>();
    }

    public class ACCSessionResult
    {
        public int bestlap { get; set; }
        public List<int> bestSplits { get; set; } = new List<int>();
        public int isWetSession { get; set; }
        public int type { get; set; }
        public List<ACCLeaderBoardLine> leaderBoardLines { get; set; } = new List<ACCLeaderBoardLine>();
    }

    public class ACCLeaderBoardLine
    {
        public ACCCarInResult car { get; set; } = new ACCCarInResult();
        public ACCDriverInResult currentDriver { get; set; } = new ACCDriverInResult();
        public int currentDriverIndex { get; set; }
        public int missingMandatoryPitstop { get; set; }
        public double[] driverTotalTimes { get; set; } = { 0, 0, 0 };
        public ACCTiming timing { get; set; } = new ACCTiming();
    }

    public class ACCCarInResult
    {
        public int carId { get; set; }
        public int raceNumber { get; set; }
        public int carModel { get; set; }
        public int cupCategory { get; set; }
        public string? teamName { get; set; }
        public string? carGroup { get; set; }
        public int nationality { get; set; }
        public int carGuid { get; set; }
        public int teamGuid { get; set; }
        public List<ACCDriverInResult> drivers { get; set; } = new List<ACCDriverInResult>();
    }

    public class ACCDriverInResult
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? shortName { get; set; }
        public string? playerId { get; set; }
        public string? UserId { get; set; }
    }

    public class ACCTiming
    {
        public double lastLap { get; set; }
        public List<int> lastSplits { get; set; } = new List<int>();
        public double bestLap { get; set; }
        public List<int> bestSplits { get; set; } = new List<int>();
        public double totalTime { get; set; }
        public int lapCount { get; set; }
        public double lastSplitId { get; set; }
    }

    public class ACCLap
    {
        public int carId { get; set; }
        public int driverIndex { get; set; }
        public int laptime { get; set; }
        public bool isValidForBest { get; set; }
        public List<int> splits { get; set; } = new List<int>();
    }

    public class ACCPenalty
    {
        public int carId { get; set; }
        public int driverIndex { get; set; }
        public string? reason { get; set; }
        public string? penalty { get; set; }
        public int penaltyValue { get; set; }
        public int violationInLap { get; set; }
        public int clearedInLap { get; set; }
    }
}
