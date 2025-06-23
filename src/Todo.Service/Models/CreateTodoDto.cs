namespace Todo.Service.Models;

public record CreateTodoDto(
    string Title,
    string Description,
    bool IsCompleted,
    DateTime DueDate
);
