using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace TuDa.CIMS.Shared;

/// <summary>
/// Base controller for CIMS Api with some extra features.
/// </summary>
public abstract class CIMSBaseController : ControllerBase
{
    /// <summary>
    /// Create ProblemDetails from <see cref="ErrorOr.Error"/>s.
    /// </summary>
    /// <param name="errors">Errors that should be used.</param>
    protected IActionResult ErrorsToProblem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Problem();
        }

        return errors.All(error => error.Type is ErrorType.Validation)
            ? ErrorsToValidationProblem(errors)
            : ErrorToProblem(errors[0]);
    }

    private IActionResult ErrorToProblem(Error error)
    {
        int statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Problem(statusCode: statusCode, title: error.Code, detail: error.Description);
    }

    private IActionResult ErrorsToValidationProblem(List<Error> errors)
    {
        var errorsDict = errors
            .GroupBy(e => e.Code)
            .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray());

        return ValidationProblem(new ValidationProblemDetails(errorsDict));
    }
}
