using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.Request;
using Todo.Api.Response;
using Todo.Service.Interfaces;
using Todo.Service.Models;

namespace Todo.Api.Controllers;

[ApiController]
[Route("api/todo")]
[Consumes("application/json")]
[Produces("application/json")]
public class TodoController(ILogger<TodoController> _logger, IMapper _mapper, ITodoService _todoService) : ControllerBase
{
    // GET: api/todo  
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<TodoItemResponse>>> GetTodos()
    {
        _logger.LogInformation("Fetching all todo items.");
        return Ok(await _todoService.GetAllTodosAsync());
    }

    // GET: api/todo/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TodoItemResponse>> GetTodoById(int id)
    {
        _logger.LogInformation("Fetching todo item with ID: {id}", id);
        var todoItem = await _todoService.GetTodoByIdAsync(id);
        if (todoItem == null)
        {
            _logger.LogWarning("Todo item with ID: {id} not found.", id);
            return NotFound();
        }
        return Ok(todoItem);
    }

    // POST: api/todo
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TodoItemResponse>> CreateTodo([FromBody] CreateTodoItemRequest todoItem, IValidator<CreateTodoItemRequest> _validator)
    {
        if (todoItem == null)
        {
            _logger.LogWarning("Received null todo item for creation.");
            return BadRequest("Todo item cannot be null.");
        }
        var validationResult = _validator.Validate(todoItem);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for todo item creation: {errors}", validationResult.Errors);
            var problemDetails = new ValidationProblemDetails(validationResult.ToDictionary())
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Error",
                Detail = "One or more validation errors occurred.",
                Instance = HttpContext.Request.Path
            };
            return ValidationProblem(problemDetails);
        }
        _logger.LogInformation("Creating a new todo item.");
        var createTodoDto = _mapper.Map<CreateTodoDto>(todoItem);
        var createdTodo = await _todoService.CreateTodoAsync(createTodoDto);
        return CreatedAtAction(nameof(GetTodoById), new { id = createdTodo.Id }, createdTodo);
    }

    // PUT: api/todo/{id}
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TodoItemResponse>> UpdateTodo(int id, [FromBody] UpdateTodoItemRequest todoItem, IValidator<UpdateTodoItemRequest> _validator)
    {
        if (todoItem == null)
        {
            _logger.LogWarning("Received null todo item for update with Id: {id}", id);
            return BadRequest("Todo item cannot be null.");
        }
        var validationResult = _validator.Validate(todoItem);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for todo item update: {errors}", validationResult.Errors);
            var problemDetails = new ValidationProblemDetails(validationResult.ToDictionary())
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Error",
                Detail = "One or more validation errors occurred.",
                Instance = HttpContext.Request.Path
            };
            return ValidationProblem(problemDetails);
        }
        if (todoItem.Id != id)
        {
            _logger.LogWarning("Received invalid todo item for update with ID: {id}", id);
            return BadRequest("Todo item is invalid.");
        }
        _logger.LogInformation("Updating todo item with Id: {id}", id);
        var updateTodoDto = _mapper.Map<UpdateTodoDto>(todoItem);
        var updatedTodo = await _todoService.UpdateTodoAsync(updateTodoDto);
        if (updatedTodo == null)
        {
            _logger.LogWarning("Todo item with Id: {id} not found for update.", id);
            return NotFound();
        }
        return Ok(updatedTodo);
    }

    // DELETE: api/todo/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        _logger.LogInformation("Deleting todo item with ID: {id}", id);
        var success = await _todoService.DeleteTodoAsync(id);
        if (!success)
        {
            _logger.LogWarning("Todo item with ID: {id} not found for deletion.", id);
            return NotFound();
        }
        return NoContent();
    }
}
