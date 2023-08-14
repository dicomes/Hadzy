using CommentsFetchStatus.MinimalApi.Models.DTO;
using FluentValidation;

namespace CommentsFetchStatus.MinimalApi.Validators;

public class FetchInfoCreateValidation : AbstractValidator<FetchInfoDto>
{
    public FetchInfoCreateValidation()
    {
        RuleFor(x => x.VideoId).NotEmpty().WithMessage("VideoId is required.");
    }
}