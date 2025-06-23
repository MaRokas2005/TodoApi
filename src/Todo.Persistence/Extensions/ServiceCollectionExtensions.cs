using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Todo.Persistence.Interfaces;

namespace Todo.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<TodoDbContext>(options =>
            options.UseInMemoryDatabase("TodoDb"));

        services.AddScoped<ITodoRepository, TodoRepository>();
        return services;
    }
}
