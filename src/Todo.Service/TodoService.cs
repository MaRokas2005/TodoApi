using AutoMapper;
using Todo.Service.Interfaces;
using Todo.Service.Models;
using Todo.Persistence.Interfaces;
using Todo.Persistence.Entities;

namespace Todo.Service;

public class TodoService(ITodoRepository _repo, IMapper _mapper) : ITodoService
{
    public async Task<IEnumerable<TodoDto>> GetAllTodosAsync()
    {
        var entities = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<TodoDto>>(entities);
    }

    public async Task<TodoDto?> GetTodoByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : _mapper.Map<TodoDto>(entity);
    }

    public async Task<TodoDto> CreateTodoAsync(CreateTodoDto dto)
    {
        var entity = _mapper.Map<TodoItem>(dto);
        var created = await _repo.AddAsync(entity);
        return _mapper.Map<TodoDto>(created);
    }

    public async Task<TodoDto?> UpdateTodoAsync(UpdateTodoDto dto)
    {
        var entity = _mapper.Map<TodoItem>(dto);
        var updated = await _repo.UpdateAsync(entity);
        return updated is null ? null : _mapper.Map<TodoDto>(updated);
    }

    public async Task<bool> DeleteTodoAsync(int id)
    {
        return await _repo.DeleteAsync(id);
    }
}
