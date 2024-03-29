using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");
        // Burada gerekirse daha fazla yapılandırma yapabilirsiniz
    });
});
//builder.Services.AddMassTransit(x =>
//{
//    x.AddConsumer<EmailMessageConsumer>();

//    x.UsingRabbitMq((context, cfg) =>
//    {
//        cfg.Host("rabbitmq://localhost");

//        cfg.ReceiveEndpoint("test_queue", e =>
//        {
//            e.ConfigureConsumer<EmailMessageConsumer>(context);
//        });
//    });
//});

//builder.Services.AddMassTransitHostedService();

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
