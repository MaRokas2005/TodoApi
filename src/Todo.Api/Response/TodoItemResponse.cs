namespace Todo.Api.Response;

public record TodoItemResponse(
    int Id,
    string Title,
    string Description,
    bool IsCompleted,
    DateTime DueDate
);
