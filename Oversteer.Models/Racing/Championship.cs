using Oversteer.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Models.Racing
{
    public class Championship
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = string.Empty;
        public string BannerImage { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string Rules { get; set; } = string.Empty;
        [Required]
        [NonEmptyGuid(ErrorMessage = "A racesim needs to be selected")]
        public Guid RaceSimId { get; set; }
        public RaceSim? RaceSim { get; set; }
        public Guid? LeagueId { get; set; }
        public League? League { get; set; }

        // Driver settings
        public bool SingleDriver { get; set; } = true;
        [Range(1, 16)]
        public int MinNrOfDrivers { get; set; } = 1;
        [Range(1, 16)]
        public int MaxNrOfDrivers { get; set; } = 1;
        [Range(1, 100)]
        public int MaxSignOutsAllowed { get; set; } = 2;
        [Range(1, 100)]
        public int MissedRacePenaltyPoints { get; set; } = 5;
        [Range(1, 100)]
        public int MaxPenaltyPoints { get; set; } = 10;
        [Range(1, 100)]
        public int MaxReservesAllowed { get; set; } = 10;

        // Registrations
        public bool AllowRegistrations { get; set; } = true;
        public bool AllowRegistrationAfterEventStart { get; set; }
        public bool AllowWildCardRegistration { get; set; }
        public bool PrivateChampionship { get; set; }
        public bool FollowersOnly { get; set; }

        // Cars
        public List<CarClass> CarClasses { get; set; } = new List<CarClass>();
        public List<Car> Cars { get; set; } = new List<Car>();

        // Features
        public List<ChampionshipFeature> ChampionshipFeatures { get; set; } = new List<ChampionshipFeature>();

        // ACC
        public ACC ACC { get; set; } = new ACC();

        // Splits
        [MinimumElements(1)]
        [ValidateComplexType]
        public List<Split> Splits { get; set; } = new List<Split>();

        // Points
        [MinimumElements(1)]
        [ValidateComplexType]
        public List<Point> Points { get; set; } = new List<Point>();
        public int FastestLapPoints { get; set; }
        public int PolePositionPoints { get;set; }
        public bool AllowTeamScoring { get; set; }
        public bool AllowManufacturerScoring { get; set; }
        public bool AllowWildCardScoring { get; set; }

        // Races
        public List<Race> Races { get; set; } = new List<Race>();
    }

    public class ChampionshipFeature
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public FeatureType FeatureType { get; set; }
    }

    public enum FeatureType
    {
        LiveBroadCasting=1,
        BroadCasting=2,
        PostRaceHighlights=3,
        Commentary=4,
        Prizes=5,
        LiveStewarding=6,
        Stewarding=7
    }
}
