using Abstractions.Models;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Sample.Consumer.Api
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly RabbitMQManager<string, string> _rabbitMqManager;
        private static RabbitMqOptions _rabbitMqOptions;
        private IModel _channel;
        private RabbitMqOptions rabbitMqOptions = new RabbitMqOptions();
        public RabbitMQConsumerService(
            IConfiguration configuration,
            RabbitMQManager<string, string> rabbitMqmanager)
        {

            _configuration = configuration;
            _rabbitMqManager = rabbitMqmanager;
            _configuration.Bind(RabbitMqOptions.OptionsSection, rabbitMqOptions);
            _rabbitMqOptions = rabbitMqOptions;
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitMqManager.Connect(_rabbitMqOptions.QueueName,
                                                _rabbitMqOptions.RoutingKey);
            _channel.BasicQos(0, 1, false);

                //BasicQos metodu, bu kanal üzerinden ne kadar mesajın alınabileceğini belirler. Bu ayar, mesajların nasıl ve ne kadar sıklıkla işleneceğini etkiler.
                //prefetchSize: İlk parametre(0) prefetch boyutunu belirtir ve bu örnekte sınırsız olduğunu gösterir.
                //prefetchCount: İkinci parametre(1) bir seferde işlenebilecek maksimum mesaj sayısını belirtir.Bu durumda, bir seferde sadece bir mesaj işlenebilir.
                //global: Üçüncü parametre(false), bu QoS ayarlarının sadece bu kanal için geçerli olduğunu gösterir(kanal bazında).

            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var routingKey = ea.RoutingKey;
                var reportMessage = JsonSerializer.Deserialize<MessageModel>(Encoding.UTF8.GetString(ea.Body.ToArray()));

                Console.WriteLine(reportMessage.Message);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_rabbitMqOptions.QueueName, false, consumer);

            return Task.CompletedTask;
        }
    }
}
