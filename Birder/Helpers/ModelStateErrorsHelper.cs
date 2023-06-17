using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Birder.Helpers;

public static class ModelStateErrorsExtensions
{
    public static string GetModelStateErrorMessages(ModelStateDictionary modelState)
    {
        string validationErrors = string.Join("; ",
                modelState.Values.Where(e => e.Errors.Count > 0)
                    .SelectMany(e => e.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray());

        return validationErrors;
    }

    public static ModelStateDictionary AddIdentityErrors(ModelStateDictionary modelState, IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            modelState.AddModelError(string.Empty, error.Description);
        }
        return modelState;
    }
}