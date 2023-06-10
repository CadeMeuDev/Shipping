using RabbitMQ.Client;

namespace Shipping.Api.Interfaces
{
    public interface IMessageBrokerProvider
    {
        public IModel Channel { get; }
    }
}