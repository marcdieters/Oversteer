namespace Oversteer.Services
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message, string queue);
    }
}
