using CAPRabbitMQ.Publisher.Api.Services;
using DotNetCore.CAP;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace CAPRabbitMQ.Publisher.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PublishMessage([FromBody] string message)
        {
            RecurringJob.AddOrUpdate<IEmailService>(
                "myrecurringjob",
                p => p.Send("Merhaba"),
                Cron.Minutely);

            return Ok();
        }
    }
}
