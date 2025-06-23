using AutoMapper;
using Todo.Persistence.Entities;
using Todo.Service.Models;

namespace Todo.Service.Profiles;

public class TodoItemProfile : Profile
{
    public TodoItemProfile()
    {
        CreateMap<TodoItem, TodoDto>();
    }
}
