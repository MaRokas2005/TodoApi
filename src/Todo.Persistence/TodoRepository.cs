using Microsoft.EntityFrameworkCore;
using Todo.Persistence.Entities;
using Todo.Persistence.Interfaces;

namespace Todo.Persistence;

public class TodoRepository(TodoDbContext _context) : ITodoRepository
{
    public async Task<IEnumerable<TodoItem>> GetAllAsync()
    {
        return await _context.TodoItems.ToListAsync();
    }

    public async Task<TodoItem?> GetByIdAsync(int id)
    {
        return await _context.TodoItems.FindAsync(id);
    }

    public async Task<TodoItem> AddAsync(TodoItem item)
    {
        var now = DateTime.UtcNow;
        var entity = item with
        {
            CreatedAt = now,
            UpdatedAt = now
        };
        var entry = await _context.TodoItems.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<TodoItem?> UpdateAsync(TodoItem item)
    {
        var existingItem = await _context.TodoItems.FindAsync(item.Id);
        if (existingItem == null)
        {
            return null;
        }
        var now = DateTime.UtcNow;
        var entity = existingItem with
        {
            Title = item.Title,
            Description = item.Description,
            IsCompleted = item.IsCompleted,
            DueDate = item.DueDate,
            UpdatedAt = now
        };
        _context.Entry(existingItem).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem is null)
        {
            return false;
        }
        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();
        return true;
    }
}
