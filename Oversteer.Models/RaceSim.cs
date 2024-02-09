using Oversteer.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Models
{
    public class RaceSim
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string FilesLocation { get; set; } = string.Empty;
        [Required]
        public string Version { get; set; }
        [Required]
        public string CurrentGameVersion { get; set; }
        public bool DoesSimSupportTrackLayouts { get; set; }
    }

    public class CarInRaceSim
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "A car needs to be selected")]
        [NonEmptyGuid]
        public Guid CarId { get; set; }
        public Car? Car { get; set; }
        [Required(ErrorMessage = "A racesim needs to be selected")]
        [NonEmptyGuid]
        public Guid RaceSimId { get; set; }
        public RaceSim? RaceSim { get; set; }
        [Required]
        public ContentType ContentType { get; set; }
        public Guid? DlcId { get; set; }
        public Dlc? Dlc { get; set; }
        public int AccCarId { get; set; }

        [NotMapped]
        public bool DisableDlc { get; set; } = true;
    }

    public class TrackInRaceSim
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "A track needs to be selected")]
        [NonEmptyGuid]
        public Guid TrackId { get; set; }
        public Track? Track { get; set; }
        [Required(ErrorMessage = "A racesim needs to be selected")]
        [NonEmptyGuid]
        public Guid RaceSimId { get; set; }
        public RaceSim? RaceSim { get; set; }
        [Required]
        public ContentType ContentType { get; set; }
        public int? DlcId { get; set; }
        public virtual Dlc? Dlc { get; set; }
        public string NameInGame { get; set; } = string.Empty;

        [NotMapped]
        public int FieldSelector { get; set; }
    }
}
