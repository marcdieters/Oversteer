using Oversteer.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Models.Racing
{
    public class Race
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "A lobby name is required")]
        public string LobbyName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime? CompleteTime { get; set; }
        public string TimeZone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string VideoLink { get; set; } = string.Empty;
        public bool ChampionshipRound { get; set; } = true;
        public bool Scheduled { get; set; }
        public bool Complete { get; set; }
        public Guid? LeagueId { get; set; }
        public League? League { get; set; }
        public Guid ResultId { get; set; }

        [MinimumElements(1)]
        [ValidateComplexType]
        public List<Session> Sessions { get; set; } = new List<Session>();

        [Required]
        [NonEmptyGuid(ErrorMessage = "A racesim needs to be selected")]
        public Guid RaceSimId { get; set; }
        public RaceSim? RaceSim { get; set; }

        [Required]
        [NonEmptyGuid(ErrorMessage = "A track needs to be selected")]
        public Guid TrackId { get; set; }
        public Track? Track { get; set; }

        public Guid? TrackLayoutId { get; set; }
        public TrackLayout? TrackLayout { get; set;}

        public Guid ChampionshipId { get; set; }
        public Championship? Championship { get; set; }

        // ACC
        public ACC Acc { get; set; } = new ACC();

        [MinimumElements(1)]
        [ValidateComplexType]
        public List<Split> Splits { get; set; } = new List<Split>();

    }
}
