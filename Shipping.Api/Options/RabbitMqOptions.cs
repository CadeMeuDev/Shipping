using System.ComponentModel.DataAnnotations;

namespace Shipping.Api.Options;
public class RabbitMqOptions
{
    public const string Name = "RabbitMq";

    [Required]
    public string HostName { get; set; } = string.Empty!;
    [Required]
    public string UserName { get; set; } = string.Empty!;
    [Required]
    public string Password { get; set; } = string.Empty!;
}
