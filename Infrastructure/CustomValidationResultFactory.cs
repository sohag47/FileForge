using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FluentValidation;
using FluentValidation.Results;
using FileForge.Entities.Base;

namespace FileForge.Infrastructure;

public class CustomValidationResultFactory : IFluentValidationAutoValidationResultFactory
{
    public Task<IActionResult?> CreateActionResult(ActionExecutingContext context, ValidationProblemDetails validationProblemDetails, IDictionary<IValidationContext, ValidationResult> validationResults)
    {
        var errors = context.ModelState
            .Where(x => x.Value!.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        var response = ApiResponse<string>.Fail("Validation failed.", errors);
        return Task.FromResult<IActionResult?>(new UnprocessableEntityObjectResult(response));
    }
}