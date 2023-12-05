using MassTransit;
using MassTransitRabbitMQ.Contracts;

namespace MassTransitRabbitMQ.Consumer.Api
{
    public class EmailMessageConsumer : IConsumer<EmailModel>
    {
        public Task Consume(ConsumeContext<EmailModel> context)
        {
            Console.WriteLine("Mesaj alındı: " + context.Message.Name);
            return Task.CompletedTask;
        }
    }
}
