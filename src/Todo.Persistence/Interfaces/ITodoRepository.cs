using Todo.Persistence.Entities;

namespace Todo.Persistence.Interfaces;

public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> GetAllAsync();
    Task<TodoItem?> GetByIdAsync(int id);
    Task<TodoItem> AddAsync(TodoItem item);
    Task<TodoItem?> UpdateAsync(TodoItem item);
    Task<bool> DeleteAsync(int id);
}
