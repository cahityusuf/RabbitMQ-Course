using DotNetCore.CAP;

namespace CAPRabbitMQ.Consumer.Api.Consomers
{
    public class EmailConsumerService:ICapSubscribe
    {
        [CapSubscribe("sample.rabbitmq.inmemory")]
        public void Consumer(string message)
        {
            Console.WriteLine($"Alınan Mesaj: {message}");
        }
    }
}
