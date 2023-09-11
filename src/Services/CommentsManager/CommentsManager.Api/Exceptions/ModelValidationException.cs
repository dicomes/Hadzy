using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CommentsManager.Api.Exceptions;

public class ModelValidationException : Exception
{
    public ModelStateDictionary ModelState { get; }

    public ModelValidationException(ModelStateDictionary modelState) 
        : base("The model validation failed.")
    {
        this.ModelState = modelState;
    }
}
