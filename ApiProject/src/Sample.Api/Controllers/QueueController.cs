using Abstractions.Models;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ;

namespace Sample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly RabbitMqPublisher<MessageModel, string, string> _publisher;
        private readonly IConfiguration configuration;
        public QueueController(RabbitMqPublisher<MessageModel, string, string> publisher, IConfiguration configuration)
        {
            _publisher = publisher;
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> QueueTest([FromBody] MessageModel message)
        {
            _publisher.Publish(
                message: message,
                queueName: configuration.GetValue<string>("RabbitMqSettings:QueueName"),
                routeKey: "test");

            return Ok();
        }
    }
}
