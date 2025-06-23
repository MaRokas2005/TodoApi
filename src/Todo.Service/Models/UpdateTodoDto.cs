namespace Todo.Service.Models;

public record UpdateTodoDto(
    int Id,
    string Title,
    string Description,
    bool IsCompleted,
    DateTime DueDate
);
