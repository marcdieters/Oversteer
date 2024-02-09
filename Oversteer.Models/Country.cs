using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Oversteer.Models
{
    public class Country
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string ImageName { get; set; } = string.Empty;
        public float GMTOffset { get; set; }
    }
}
