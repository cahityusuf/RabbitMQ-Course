using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace CAPRabbitMQ.Publisher.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly ICapPublisher _capPublisher;

        public EmailController(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        [HttpPost]
        public async Task<IActionResult> PublishMessage([FromBody] string message)
        {
            await _capPublisher.PublishAsync("sample.rabbitmq.inmemory", message);

            return Ok();
        }
    }
}
