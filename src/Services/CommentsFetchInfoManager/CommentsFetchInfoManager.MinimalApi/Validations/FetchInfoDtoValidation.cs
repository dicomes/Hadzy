using CommentsFetchInfoManager.MinimalApi.Models.DTO;
using FluentValidation;

namespace CommentsFetchInfoManager.MinimalApi.Validations;

public class FetchInfoDtoValidation : AbstractValidator<FetchInfoDto>
{
    public FetchInfoDtoValidation()
    {
        RuleFor(x => x.VideoId).NotEmpty().WithMessage("VideoId is required.");
    }
}