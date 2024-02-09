using Newtonsoft.Json;
using Oversteer.Models;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Server
{
    public static class MessageHandler
    {
        public static void NewMessage(string body)
        {
            HostEventMessage hostEventMessage = JsonConvert.DeserializeObject<HostEventMessage>(body);

            switch (hostEventMessage.HostEventType)
            {
                case HostEventType.CreateServer:
                    Server.Create(hostEventMessage.Body);
                    break;
            }
        }
    }
}
