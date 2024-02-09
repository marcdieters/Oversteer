using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Models
{
    public class LeagueUser
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "A league needs to be selected")]
        public Guid LeagueId { get; set; }
        public League? League { get; set; }
        public string UserId { get; set; }
        public bool PremiumMember { get; set; }
        public bool Approved { get; set; }
        public bool Banned { get; set; }
    }
}
