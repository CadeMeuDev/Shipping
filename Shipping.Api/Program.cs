using Microsoft.Extensions.Options;
using Shipping.Api.Interfaces;
using Shipping.Api.Options;
using Shipping.Api.Providers;
using Shipping.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var services = builder.Services;

services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.Name));
services.Configure<QueuesOptions>(configuration.GetSection(QueuesOptions.Name));

services.AddSingleton<IMessageBrokerProvider, RabbitMqProvider>();
services.AddSingleton<IMessageBroker, MessageBrokerService>();

services.AddControllers();
services.AddRouting(options => options.LowercaseUrls = true);
services.AddEndpointsApiExplorer();

services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var queuesOptions = scope.ServiceProvider.GetService<IOptions<QueuesOptions>>();
    _ = queuesOptions ?? throw new Exception();

    var messageBroker = scope.ServiceProvider.GetService<IMessageBroker>();
    _ = messageBroker ?? throw new Exception();

    queuesOptions.Value.Queues.ForEach(q => messageBroker.CreateQueue(q));
}

app.Run();
