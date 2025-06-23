using FluentValidation;
using Todo.Api.Request;

namespace Todo.Api.Validators;

public class UpdateTodoItemRequestValidator : AbstractValidator<UpdateTodoItemRequest>
{
    public UpdateTodoItemRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Id is required.")
            .GreaterThan(0).WithMessage("Id must be greater than 0.");
        
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
