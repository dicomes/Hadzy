using FluentValidation.Results;

namespace CommentsFetchInfoManager.MinimalApi.Validations;

public interface IValidationService<T>
{
    Task<ValidationResult> ValidateAsync(T entity);
}