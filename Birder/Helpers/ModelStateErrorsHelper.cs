using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace Birder.Helpers
{
    public static class ModelStateErrorsExtensions
    {
        //ToDo: change to an extension method?
        public static string GetModelStateErrorMessages(ModelStateDictionary modelState)
        {
            string validationErrors = string.Join("; ",
                    modelState.Values.Where(E => E.Errors.Count > 0)
                        .SelectMany(E => E.Errors)
                        .Select(E => E.ErrorMessage)
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
}
