using Microsoft.AspNetCore.Mvc;

namespace Todo.Api.Controllers;

[ApiController]
[Route("api/todo")]
[Consumes("application/json")]
[Produces("application/json")]
public class TodoController : ControllerBase
{
}
