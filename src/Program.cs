using Microsoft.EntityFrameworkCore;
using TodoApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoDbContext>(opt => opt.UseInMemoryDatabase("Todos"));

var app = builder.Build();
Log.Information($"Application has been built for {builder.Environment.EnvironmentName} environment.");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(() => Log.Information("App is running."));
app.Lifetime.ApplicationStopping.Register(() => Log.Warning("App is shutting down."));

app.Run();
