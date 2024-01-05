using CAPRabbitMQ.Consumer.Api.Consomers;
using DotNetCore.CAP;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCap(x =>
{
    x.UseSqlServer(opt => {
        opt.ConnectionString = builder.Configuration.GetConnectionString("CapConnectionString");
    }); // In-memory storage kullanýmý
    x.UseRabbitMQ("localhost");
    x.UseDashboard(); // CAP Dashboard'ý etkinleþtirir
                      // Diðer CAP yapýlandýrmalarý...
});

builder.Services.AddTransient<ICapSubscribe, EmailConsumerService>();

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
