using Oversteer.Models.Racing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Models
{
    public class ACC
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Requirements
        public int TrackRating { get; set; } = -1;
        public int SafetyRating { get; set; } = -1;
        public int RaceCraftRating { get; set; } = -1;

        // Generic setting
        public int FormationLapType { get; set; }

        // Race settings
        public int MandatoryPitstopCount { get; set; }
        public bool RefuelAllowedDuringRace { get; set; }
        public bool RefuelTimeFixed { get; set; }
        public bool MandatoryPitStopFuel { get; set; }
        public bool MandatoryPitStopTyres { get; set; }
        public bool MandatoryPitStopDriverChange { get; set; }
        public bool ShortFormationLap { get; set; }

        // Assist rules
        public int StabilityControlLevelMax { get; set; }
        public bool DisableAutoSteer { get; set; } = true;
        public bool DisableAutoLight { get; set; } = false;
        public bool DisableAutoWiper { get; set; } = false;
        public bool DisableAutoEngineStart { get; set; } = false;
        public bool DisableAutoPitLimiter { get; set; } = false;
        public bool DisableAutoClutch { get; set; } = false;
        public bool DisableIdealLie { get; set; } = true;
        public bool DisableAutoGear { get; set; } = true;

        // Weather 
        [Range(10, 40)]
        public int AmbientTemp { get; set; } = 25;
        [Range(0,1)]
        public float CloudLevel { get; set; }
        [Range(0, 1)]
        public float Rain { get; set; }
        [Range(0, 7)]
        public int WeatherRandomness { get; set; } = 1;
    }

    public enum FormationLapType
    {
        [Display(Name = "Sefault formation lap with position control and UI")]
        DefaultFormationLapWithPositionControlandUI = 3,
        [Display(Name = "Old limiter lap")]
        Free = 0,
        [Display(Name = "Free (Replaces /manual start), only usable for private servers")]
        OldLimitedLap = 1,
    }
}
