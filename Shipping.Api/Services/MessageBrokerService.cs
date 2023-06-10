using System.Text;
using RabbitMQ.Client;
using Shipping.Api.Interfaces;

namespace Shipping.Api.Services
{
    public class MessageBrokerService : IMessageBroker
    {
        private readonly IModel _channel;
        private readonly ILogger<MessageBrokerService> _logger;
        public MessageBrokerService(IMessageBrokerProvider provider, ILogger<MessageBrokerService> logger)
        {
            _channel = provider.Channel;
            _logger = logger;
        }

        public bool CreateQueue(string queueName)
        {
            try
            {
                _channel.QueueDeclare(queue: queueName,
                           durable: false,
                           exclusive: false,
                           autoDelete: false,
                           arguments: null);

                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Problem create {@Queue}", queueName);
                return false;
            }

        }

        public bool PublishMessage(object message, string queueName)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(message);
            try
            {
                var body = Encoding.UTF8.GetBytes(json);
                _channel.BasicPublish(exchange: string.Empty,
                     routingKey: queueName,
                     basicProperties: null,
                     body: body);
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Problem send {@Message} on  {@Queue}", json, queueName);
                return false;
            }
        }
    }
}