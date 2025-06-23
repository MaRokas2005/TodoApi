using Todo.Service.Models;

namespace Todo.Service.Interfaces;

public interface ITodoService
{
    Task<IEnumerable<TodoDto>> GetAllTodosAsync();
    Task<TodoDto?> GetTodoByIdAsync(int id);
    Task<TodoDto> CreateTodoAsync(CreateTodoDto dto);
    Task<TodoDto?> UpdateTodoAsync(UpdateTodoDto dto);
    Task<bool> DeleteTodoAsync(int id);
}
