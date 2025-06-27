using Microsoft.EntityFrameworkCore;
using Todo.Persistence.Entities;

namespace Todo.Persistence;

public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems { get; set; }
}
