using CAPRabbitMQ.Publisher.Api.Services;
using Hangfire;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("CapConnectionString")));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();

builder.Services.AddCap(x =>
{
    x.UseSqlServer(opt => {
        opt.ConnectionString = builder.Configuration.GetConnectionString("CapConnectionString");
    }); // In-memory storage kullanýmý
    x.UseRabbitMQ("localhost");
    x.UseDashboard(); // CAP Dashboard'ý etkinleþtirir
                      // Diðer CAP yapýlandýrmalarý...
});

builder.Services.AddScoped<IEmailService,EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHangfireDashboard();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHangfireDashboard();
app.Run();
