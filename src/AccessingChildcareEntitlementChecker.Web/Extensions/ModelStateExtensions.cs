using Microsoft.AspNetCore.Mvc.ModelBinding;
using FluentValidation.Results;

namespace AccessingChildcareEntitlementChecker.Web.Extensions;

public static class ModelStateExtensions
{
    public static void AddValidationErrors(this ModelStateDictionary modelState, ValidationResult result)
    {
        foreach (var error in result.Errors)
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
    }
}
