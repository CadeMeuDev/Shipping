namespace Shipping.Api.Interfaces;

public interface IMessageBroker
{
    bool CreateQueue(string queueName);
    bool PublishMessage(object message, string queueName);
}
