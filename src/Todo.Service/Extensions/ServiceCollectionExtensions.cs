using Microsoft.Extensions.DependencyInjection;
using Todo.Persistence.Extensions;
using Todo.Service.Interfaces;

namespace Todo.Service.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTodoServices(this IServiceCollection services)
    {
        services.AddPersistence();
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
        services.AddScoped<ITodoService, TodoService>();
        return services;
    }
}
