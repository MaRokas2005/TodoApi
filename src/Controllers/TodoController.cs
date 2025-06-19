using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Dtos;

namespace src.Controllers;

[ApiController]
[Route("api/todo")]
[Consumes("application/json")]
[Produces("application/json")]
public class TodoController(ILogger<TodoController> _logger, TodoDbContext _context) : ControllerBase
{
    // GET: api/todo
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
    {
        _logger.LogInformation("Fetching all todos");
        return await _context.Todos.ToListAsync();
    }

    // GET: api/todo/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Todo>> GetTodo(int id)
    {
        _logger.LogInformation("Fetching todo with ID: {id}", id);
        var todo = await _context.Todos.FindAsync(id);

        if (todo == null)
        {
            _logger.LogWarning("Todo with ID: {id} not found", id);
            return NotFound();
        }
        _logger.LogInformation("Todo with ID: {id} found. {@todo}", id, todo);
        return todo;
    }

    // PUT: api/todo/{id}
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutTodo(int id, UpdateTodoDto todo)
    {
        try
        {
            _logger.LogInformation("Updating todo with ID: {id}", id);
            var existingTodo = await _context.Todos.FindAsync(id);
            if (existingTodo == null)
            {
                _logger.LogWarning("Todo with ID: {id} not found for update", id);
                return NotFound();
            }

            if (todo.Title is not null) existingTodo.Title = todo.Title;
            if (todo.Description is not null) existingTodo.Description = todo.Description;
            if (todo.IsCompleted.HasValue) existingTodo.IsCompleted = todo.IsCompleted.Value;
            if (todo.DueDate.HasValue) existingTodo.DueDate = todo.DueDate.Value;
            existingTodo.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Todo with ID: {id} updated successfully", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await (_context.Todos.AnyAsync(e => e.Id == id))))
                {
                    _logger.LogWarning("Todo with ID: {id} not found during update", id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError("Concurrency error while updating todo with ID: {id}", id);
                    throw;
                }
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating todo with ID: {id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the todo.");
        }
    }
    // POST: api/todo
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Todo>> PostTodo(CreateTodoDto todo)
    {
        try
        {
            Todo newTodo = new Todo
            {
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                DueDate = todo.DueDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Todos.Add(newTodo);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created new todo: {@todo}", newTodo);

            return CreatedAtAction("GetTodo", new { id = newTodo.Id }, newTodo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating new todo");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the todo.");
        }
    }

    // DELETE: api/todo/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        try
        {
            _logger.LogInformation("Deleting todo with ID: {id}", id);
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                _logger.LogWarning("Todo with ID: {id} not found for deletion", id);
                return NotFound();
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Todo with ID: {id} deleted successfully", id);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting todo with ID: {id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the todo.");
        }
    }
}
