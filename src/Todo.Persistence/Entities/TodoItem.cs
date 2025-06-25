namespace Todo.Persistence.Entities;

public record TodoItem(
    int Id, 
    string Title, 
    string? Description, 
    bool IsCompleted, 
    DateTime DueDate, 
    DateTime CreatedAt, 
    DateTime UpdatedAt
);