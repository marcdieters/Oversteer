using Microsoft.AspNetCore.Identity;
using Oversteer.Validators;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace Oversteer.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        public bool VerifiedSteamId { get; set; } = false;
        public string? SteamId { get; set; }
        [Required(ErrorMessage = "A carbrand needs to be selected")]
        [NonEmptyGuid]
        public Guid CountryId { get; set; }
        public Country? Country { get; set; }
        public string? DiscordId { get; set; }
        public DateTime? ValidUntil { get; set; }
        public bool Enabled { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool SubscribeToNewsLetter { get; set; } = false;
        public float GMTOffset { get; set; }
        public bool TimezoneVerified { get; set; } = false;
        public string ServerAuthKey { get; set; } = string.Empty;
        public DateTime? ServerKeyValidUntil { get; set; }
    }
}
