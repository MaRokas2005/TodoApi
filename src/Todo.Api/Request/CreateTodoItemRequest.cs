namespace Todo.Api.Request;

public record CreateTodoItemRequest(
    string Title, 
    string? Description, 
    bool? IsCompleted, 
    DateTime DueDate
);
