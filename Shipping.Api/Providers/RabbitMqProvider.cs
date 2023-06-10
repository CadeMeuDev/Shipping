using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Shipping.Api.Options;
using Shipping.Api.Interfaces;

namespace Shipping.Api.Providers;

public class RabbitMqProvider : IMessageBrokerProvider
{
    public IModel Channel { get; }

    public RabbitMqProvider(IOptions<RabbitMqOptions> rabbitMqOptions, ILogger<RabbitMqProvider> logger)
    {
        var value = rabbitMqOptions.Value;
        var factory = new ConnectionFactory
        {
            HostName = value.HostName,
            UserName = value.UserName,
            Password = value.Password,
        };

        var connection = factory.CreateConnection();
        Channel = connection.CreateModel();

        logger.LogInformation("RabbitMqProvider Channel is open: {@IsOpen}", Channel.IsOpen);
    }
}