using Oversteer.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Models
{
    public class LeagueRegistration
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string LeagueName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? URL { get; set; }
        public NrOfUsers NrOfUsers { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A plan needs to be selected")]
        [NonEmptyGuid]
        public Guid PlanId { get; set; }
        public Plan? Plan { get; set; }
    }

    public enum NrOfUsers
    {
        LessThenTen,
        TenToHunderd,
        HunderdToFiveHundred
    }
}
