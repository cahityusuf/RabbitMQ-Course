using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace RabbitMQ
{
    public class RabbitMQManager<TQueueName, TRouteKey> : IDisposable
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IConfiguration _configuration;
        private static RabbitMqOptions _rabbitMqOptions;
        private RabbitMqOptions rabbitMqOptions = new RabbitMqOptions();
        public RabbitMQManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.Bind(RabbitMqOptions.OptionsSection, rabbitMqOptions);
            _rabbitMqOptions = rabbitMqOptions;
        }

        public IModel Connect(TQueueName queueName, TRouteKey routeKey)
        {
            var factory = new ConnectionFactory { HostName = _configuration.GetValue<string>("RabbitMqOptions:ConnectionString") };
            _connection = factory.CreateConnection();

            if (_channel is { IsOpen: true })
            {
                return _channel;
            }

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(
                            exchange: _rabbitMqOptions.ExchangeName, 
                            type: ExchangeType.Direct,
                            durable: true,
                            autoDelete: false);

            _channel.QueueDeclare(
                            queue: queueName.ToString(),
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

            _channel.QueueBind(
                            exchange: _rabbitMqOptions.ExchangeName, 
                            queue: queueName.ToString(), 
                            routingKey: routeKey.ToString());

            return _channel;
        }


        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();

            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
