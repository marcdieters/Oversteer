using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Oversteer.Models
{
    public class UserForAuthenticationDto
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public string UniqueId { get; set; }
        public string Description { get; set; }
    }
    public class AuthResponseDto
    {
        public bool IsAuthSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string UnId { get; set; }
        public byte[] Photo { get; set; }
        public bool Successful { get; set; }
        public string Error { get; set; }
        public string UserId { get; set; }
    }

    public class RefreshTokenDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string DeviceId { get; set; }
    }
}
