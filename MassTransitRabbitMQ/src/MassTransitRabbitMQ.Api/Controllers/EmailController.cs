using MassTransit;
using MassTransitRabbitMQ.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitRabbitMQ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EmailController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmailModel message)
        {
            await _publishEndpoint.Publish(message);
            return Accepted();
        }
    }
}
