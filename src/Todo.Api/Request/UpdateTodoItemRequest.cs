namespace Todo.Api.Request;

public record UpdateTodoItemRequest(
    int? Id,
    string Title,
    string Description,
    bool? IsCompleted,
    DateTime? DueDate
);
