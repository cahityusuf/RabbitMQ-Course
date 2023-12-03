using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "task_queue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

var message = "Gönderilen mesaj";


var properties = channel.CreateBasicProperties();
properties.Persistent = true;
for (int i = 0; i < 20; i++)
{
    var sayac = i+1;
    var body = Encoding.UTF8.GetBytes(message + "=> " + sayac);
    channel.BasicPublish(exchange: string.Empty,
                         routingKey: "task_queue",
                         basicProperties: properties,
                         body: body);
}

Console.WriteLine($" [x] Sent {message}");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

