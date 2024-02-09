using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Oversteer.Services
{
    public class RabbitMQProducer : IMessageProducer
    {
        public void SendMessage<T>(T message, string queue)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: queue, body: body);
        }
    }
}
