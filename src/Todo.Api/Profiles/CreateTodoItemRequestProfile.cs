using AutoMapper;
using Todo.Api.Request;
using Todo.Service.Models;

namespace Todo.Api.Profiles;

public class CreateTodoItemRequestProfile : Profile
{
    public CreateTodoItemRequestProfile()
    {
        CreateMap<CreateTodoItemRequest, CreateTodoDto>();
    }
}
