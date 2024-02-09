using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Models
{
    public class HostEventMessage
    {
        public HostEventType HostEventType { get; set; }
        public string Body { get; set; } = string.Empty;
    }

    public enum HostEventType
    {
        CreateServer=1,
        StartServer=2,
        StopServer=3,
        UpdateEntryList=4,
        GetLogFile=5,
        GetConfigFiles=6
    }
}
