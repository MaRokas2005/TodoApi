using FluentValidation;
using Todo.Api.Request;

namespace Todo.Api.Validators;

public class CreateTodoItemRequestValidator : AbstractValidator<CreateTodoItemRequest>
{
    public CreateTodoItemRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        RuleFor(x => x.IsCompleted)
            .NotNull().WithMessage("IsCompleted must be specified.");
        RuleFor(x => x.DueDate)
            .NotNull().WithMessage("Due date is required.")
            .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.");
    }
}
