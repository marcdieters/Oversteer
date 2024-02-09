using Oversteer.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Models
{
    public class Host
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string FriendlyName { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public Guid ApplicationUserId { get; set; }
        public Guid? CountryId { get; set; }
        public Country? Country { get; set; }
        public Guid? LeagueId { get; set; }
        public League? League { get; set; }
        public int Priority { get; set; }
        public DateTime Ping { get; set; }
        public string QueueName { get; set; } = string.Empty;
        [IntCanBeZeroOrInRange(1025, 65536)]
        public int TcpStartPort { get; set; }
        [IntCanBeZeroOrInRange(1025, 65536)]
        public int TcpEndPort { get; set; }
        [IntCanBeZeroOrInRange(1025, 65536)]
        public int UdpStartPort { get; set; }
        [IntCanBeZeroOrInRange(1025, 65536)] 
        public int UdpEndPort { get; set; }
        [IntCanBeZeroOrInRange(1025, 65536)]
        public int HttpStartPort { get; set; }
        [IntCanBeZeroOrInRange(1025, 65536)]
        public int HttpEndPort { get; set; }
        public List<Server> Servers { get; set; } = new List<Server>();

        [NotMapped]
        public bool ReadyForPlacement
        {
            get 
            { 
                if (TcpStartPort > 1024 && TcpEndPort > 1024 && UdpStartPort > 1024 && UdpEndPort > 1024)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set { }
        }

        [NotMapped]
        public bool Disabled
        {
            get { return !ReadyForPlacement; }
            set { }
        }
    }
}
