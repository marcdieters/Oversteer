using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Models
{
    public class HostPreAuthDto
    {
        public Guid HostId { get; set; }
        public string HostSecret { get; set; } = string.Empty;
    }

    public class HostPreAuthResponse
    {
        public Guid HostId { get; set; }
        public string Token { get; set; } = string.Empty;
    }

    public class HostLogin
    {
        public Guid HostId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string HostSecret { get; set; } = string.Empty;
    }
}
