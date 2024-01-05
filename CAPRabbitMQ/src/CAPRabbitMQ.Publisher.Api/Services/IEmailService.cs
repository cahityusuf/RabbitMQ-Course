namespace CAPRabbitMQ.Publisher.Api.Services
{
    public interface IEmailService
    {
        Task Send(string message);
    }
}
