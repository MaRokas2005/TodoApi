namespace Todo.Service.Models;

public record TodoDto(
    int Id, 
    string Title, 
    string? Description, 
    bool IsCompleted, 
    DateTime DueDate
);