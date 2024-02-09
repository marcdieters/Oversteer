using System;
using System.Collections.Generic;
using System.Text;

namespace Oversteer.Models
{
    public class LoginViewModel
    {
        public string JwtToken { get; set; }
        public string PasswordHash { get; set; }
        public string UnId { get; set; }
        public byte[] Photo { get; set; }
        public bool Successful { get; set; }
        public string Error { get; set; }
        public string UserId { get; set; }
    }
}
