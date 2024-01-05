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
    }); // In-memory storage kullan�m�
    x.UseRabbitMQ("localhost");
    x.UseDashboard(); // CAP Dashboard'� etkinle�tirir
                      // Di�er CAP yap�land�rmalar�...
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
