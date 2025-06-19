using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Dtos;

namespace TodoApi.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController(ILogger<TodoController> _logger, TodoDbContext _context) : ControllerBase
    {
        // GET: api/todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            _logger.LogInformation("Fetching all todos");
            return await _context.Todos.ToListAsync();
        }

        // GET: api/todo/{id}
        [HttpGet("{id}")]
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
        public async Task<IActionResult> PutTodo(int id, UpdateTodoDto todo)
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
                if (!TodoExists(id))
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

        // POST: api/todo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo(CreateTodoDto todo)
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

        // DELETE: api/todo/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
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

        private bool TodoExists(int id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }
    }
}
