using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Models
{
    public class RefreshToken
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string DeviceId { get; set; }
        [Required]
        public string Token { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
