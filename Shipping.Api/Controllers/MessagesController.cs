using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shipping.Api.Interfaces;
using Shipping.Api.Options;

namespace Shipping.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMessageBroker _messageBroker;
    private readonly ILogger<MessagesController> _logger;
    private readonly QueuesOptions _queueOptions;

    public MessagesController(
        IMessageBroker messageBroker,
        IOptions<QueuesOptions> queueOptions,
        ILogger<MessagesController> logger)
    {
        _messageBroker = messageBroker;
        _logger = logger;
        _queueOptions = queueOptions.Value;
    }

    [HttpPost]
    public IActionResult Post([FromBody] object message)
    {

        _logger.LogInformation("Queue {@Qeeue} Created.", _queueOptions.Unprocessed);
        var uuid = Guid.NewGuid();
        var sendMessage = new
        {
            uuid,
            message
        };

        var messageSended = _messageBroker.PublishMessage(sendMessage, _queueOptions.Unprocessed);

        if (messageSended)
        {
            _logger.LogInformation("Message {@Message} sended.", message);
            return Created($"https://shipping.com/{uuid}", sendMessage);
        }

        _logger.LogWarning("Message {@Message} not sended.", message);

        var error = new
        {
            Message = message,
            Description = "Message not sended."
        };
        return BadRequest(error);
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}
