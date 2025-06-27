using AutoMapper;
using Todo.Api.Response;
using Todo.Service.Models;

namespace Todo.Api.Profiles;

public class TodoDtoProfile : Profile
{
    public TodoDtoProfile()
    {
        CreateMap<TodoDto, TodoItemResponse>();
    }
}
