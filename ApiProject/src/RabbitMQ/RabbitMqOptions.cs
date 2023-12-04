namespace RabbitMQ
{
    public class RabbitMqOptions
    {
        public const string OptionsSection = "RabbitMqOptions";
        public string? ConnectionString { get; set; }
        public string? QueueName { get; set; }
        public string? ExchangeName { get; set; }
        public string? ExchangeType { get; set; } = string.Empty;
        public string? RoutingKey { get; set; }
    }
}
