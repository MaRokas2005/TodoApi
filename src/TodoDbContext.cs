using Microsoft.EntityFrameworkCore;

namespace src;

public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos { get; set; }
}
