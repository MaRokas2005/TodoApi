using AutoMapper;
using Todo.Api.Request;
using Todo.Service.Models;

namespace Todo.Api.Profiles;

public class UpdateTodoItemRequestProfile : Profile
{
    public UpdateTodoItemRequestProfile()
    {
        CreateMap<UpdateTodoItemRequest, UpdateTodoDto>();
    }
}
