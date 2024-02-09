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
    public class CarBrand
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        [Required(ErrorMessage = "A country needs to be selected")]
        [NonEmptyGuid]
        public Guid CountryId { get; set; }
        public Country? Country { get; set; }
    }
}
