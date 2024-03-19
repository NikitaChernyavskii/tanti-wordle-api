using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Wordle.Exceptions;

namespace Wordle.Api.Infrastructure.ExceptionHandlers;

[ExcludeFromCodeCoverage]
public class ValidationFailedExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ValidationFailedExceptionHandler> _logger;

    public ValidationFailedExceptionHandler(ILogger<ValidationFailedExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationFailedException
            && exception is not ValidationException)
        {
            return false;
        }

        _logger.LogError(exception, $"Exception(Validation) details: {exception.Message}");

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "A validation error occurred.",
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails);

        return true;
    }
}
