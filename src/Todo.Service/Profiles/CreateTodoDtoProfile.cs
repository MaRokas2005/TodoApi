using AutoMapper;
using Todo.Persistence.Entities;
using Todo.Service.Models;

namespace Todo.Service.Profiles;

public class CreateTodoDtoProfile : Profile
{
    public CreateTodoDtoProfile()
    {
        CreateMap<CreateTodoDto, TodoItem>()
            .ConstructUsing(src => new TodoItem(
                0, 
                src.Title,
                src.Description,
                src.IsCompleted,
                src.DueDate,
                DateTime.UtcNow,
                DateTime.UtcNow
                ));
    }
}
