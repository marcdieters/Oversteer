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
    public class Dlc
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Url { get; set; } = string.Empty;
        [Required(ErrorMessage = "A racesim needs to be selected")]
        [NonEmptyGuid]
        public Guid RaceSimId { get; set; }
        public RaceSim? RaceSim { get; set; }
        [Required]
        public ContentType ContentType { get; set; }
        [Required]
        public bool Free { get; set; }
    }
}
