
using DotNetCore.CAP;

namespace CAPRabbitMQ.Publisher.Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly ICapPublisher _capPublisher;

        public EmailService(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public async Task Send(string message)
        {
            await _capPublisher.PublishAsync("sample.rabbitmq.inmemory", message);
        }
    }
}
