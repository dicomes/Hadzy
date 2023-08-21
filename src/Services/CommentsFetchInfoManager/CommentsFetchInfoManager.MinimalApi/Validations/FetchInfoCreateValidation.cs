using CommentsFetchInfoManager.MinimalApi.Models.DTO;
using FluentValidation;

namespace CommentsFetchInfoManager.MinimalApi.Validations;

public class FetchInfoCreateValidation : AbstractValidator<FetchInfoDto>
{
    public FetchInfoCreateValidation()
    {
        RuleFor(x => x.VideoId).NotEmpty().WithMessage("VideoId is required.");
    }
}