using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMQ
{
    public class RabbitMqPublisher<TMessage, TQueueName, TRouteKey>
    {
        private readonly RabbitMQManager<TQueueName, TRouteKey> _rabbitMqService;
        private readonly IConfiguration _configuration;
        private static RabbitMqOptions _rabbitMqOptions;
        private RabbitMqOptions rabbitMqOptions = new RabbitMqOptions();

        public RabbitMqPublisher(RabbitMQManager<TQueueName, TRouteKey> rabbitMqService, IConfiguration configuration)
        {
            _rabbitMqService = rabbitMqService;

            _configuration = configuration;
            _configuration.Bind(RabbitMqOptions.OptionsSection, rabbitMqOptions);
            _rabbitMqOptions = rabbitMqOptions;
        }

        public void Publish(TMessage message, TQueueName queueName, TRouteKey routeKey)
        {
            var channel = _rabbitMqService.Connect(queueName, routeKey);

            var bodyString = JsonSerializer.Serialize(message);

            var bodyByte = Encoding.UTF8.GetBytes(bodyString);
            var properties = channel.CreateBasicProperties();

            properties.Persistent = true;

            channel.BasicPublish(exchange: _rabbitMqOptions.ExchangeName,
                routingKey: routeKey.ToString(), true, basicProperties: properties, body: bodyByte);
        }
    }
}
