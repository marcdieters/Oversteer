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
    public class Car
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Image { get; set; } = string.Empty;
        [Required(ErrorMessage = "A carbrand needs to be selected")]
        [NonEmptyGuid]
        public Guid CarBrandId { get; set; }
        public CarBrand? CarBrand { get; set; }
        [Required(ErrorMessage = "A car class needs to be selected")]
        [NonEmptyGuid]
        public Guid CarClassId { get; set; }
        public CarClass? CarClass { get; set; }

        [NotMapped]
        public bool Selected { get;set; }
    }
}
