using CommentsFetchInfoManager.MinimalApi.Models.DTO;
using FluentValidation;
using FluentValidation.Results;

namespace CommentsFetchInfoManager.MinimalApi.Validations;

public class FetchInfoValidationService : IValidationService<FetchInfoDto> 
{
    private readonly IValidator<FetchInfoDto> _validator;

    public FetchInfoValidationService(IValidator<FetchInfoDto> validator)
    {
        _validator = validator;
    }

    public async Task<ValidationResult> ValidateAsync(FetchInfoDto fetchInfoDto)
    {
        return await _validator.ValidateAsync(fetchInfoDto);
    }
}