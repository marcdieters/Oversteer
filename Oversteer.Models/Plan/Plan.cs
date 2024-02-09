using Oversteer.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Models
{
    public class Plan
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public List<FeatureInPlan> Features { get; set; } = new List<FeatureInPlan>();
    }

    public class FeatureInPlan
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "A plan needs to be selected")]
        [NonEmptyGuid]
        public Guid PlanId { get; set; }
        public Plan? Plan { get; set; }
        [Required(ErrorMessage = "A feature needs to be selected")]
        [NonEmptyGuid]
        public Guid FeatureId { get; set; }
        public Feature? Feature { get; set; }
        [Required(ErrorMessage = "A category needs to be selected")]
        [NonEmptyGuid]
        public Guid CategoryId { get; set; }
        public PlanCategory? PlanCategory { get; set; }

        [NotMapped]
        public int FieldSelector { get; set; }
    }
}
