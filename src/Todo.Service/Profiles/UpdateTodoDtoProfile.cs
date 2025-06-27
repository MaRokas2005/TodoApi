using AutoMapper;
using Todo.Persistence.Entities;
using Todo.Service.Models;

namespace Todo.Service.Profiles;

public class UpdateTodoDtoProfile : Profile
{
    public UpdateTodoDtoProfile()
    {
        CreateMap<UpdateTodoDto, TodoItem>()
            .ConstructUsing(src => new TodoItem(
                src.Id,
                src.Title,
                src.Description,
                src.IsCompleted,
                src.DueDate,
                DateTime.UtcNow,
                DateTime.UtcNow
            ));
    }
}
