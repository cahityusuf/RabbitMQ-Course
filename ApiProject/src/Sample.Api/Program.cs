using RabbitMQ;
using RabbitMQ.Client;
using Sample.Api.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// RabbitMQManager örneðinizi kaydedin
builder.Services.AddSingleton(typeof(RabbitMQManager<,>));

// RabbitMqPublisher için dinamik kayýt
builder.Services.AddSingleton(typeof(RabbitMqPublisher<,,>));

builder.Services.AddHostedService<RabbitMQConsumerService>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
