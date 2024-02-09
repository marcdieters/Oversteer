using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oversteer.Validators;

namespace Oversteer.Models
{
    public class Track
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required(ErrorMessage = "A plan needs to be selected")]
        [NonEmptyGuid]
        public Guid CountryId { get; set; }
        public Country? Country { get; set; }
        public DateTime LastUpdate { get; set; }
        public string SceneryImage { get; set; } = string.Empty;
        public List<TrackLayout> TrackLayouts { get; set; } = new List<TrackLayout>();
        public List<TrackInRaceSim> TrackInRaceSims { get; set; } = new List<TrackInRaceSim>();
    }

    public class TrackLayout
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "A track needs to be selected")]
        [NonEmptyGuid]
        public Guid TrackId { get; set; }
        public Track? Track { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string GameName { get; set; } = string.Empty;
        public string LayoutImage { get; set; } = string.Empty;
        [Required(ErrorMessage = "A racesim needs to be selected")]
        [NonEmptyGuid]
        public Guid RaceSimId { get; set; }
        public RaceSim? RaceSim { get; set; }
        public int Pitboxes { get; set; }
        public int PrivateServerSlots { get; set; }
        public byte[]? DataFile { get; set; }
        public bool IsSeperateContent { get; set; }

        [NotMapped]
        public int FieldSelector { get; set; }
    }
}
