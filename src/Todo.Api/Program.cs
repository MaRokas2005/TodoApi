using FluentValidation;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Todo.Api.Infrastructure;
using Todo.Api.Validators;
using Todo.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddTodoServices();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<CreateTodoItemRequestValidator>();
builder.Services.AddFluentValidationAutoValidation(cfg =>
{
    cfg.DisableBuiltInModelValidation = true;
});

var app = builder.Build();
Log.Information($"Application has been built for {builder.Environment.EnvironmentName} environment.");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(() => Log.Information("App is running."));
app.Lifetime.ApplicationStopping.Register(() => Log.Warning("App is shutting down."));

app.Run();
