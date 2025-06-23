using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Todo.Api.Infrastructure;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> _logger
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken
    )
    {
        _logger.LogError(exception, "Exception occured: {Message}", exception.Message);
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server error",
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-500-internal-server-error"
        };
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(
            problemDetails, 
            cancellationToken: cancellationToken
        );
        return true;
    }
}
