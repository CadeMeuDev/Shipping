using System.ComponentModel.DataAnnotations;


namespace Shipping.Api.Options
{
    public class QueuesOptions
    {
        public const string Name = "Queues";

        [Required]
        public string Unprocessed { get; set; } = string.Empty!;

        public List<string> Queues => new()
        {
            Unprocessed
        };
    }
}