using CommentsManager.Api.DTO;

namespace CommentsManager.Api.Factories;

public static class CommentsPageResponseFactory
{
    public static CommentsPageResponse Create(
        IEnumerable<CommentForResponse> filteredComments, QueryForCommentsPage query, int totalCommentsCount)
    {
        var filteredCommentsList = filteredComments.ToList();
        int pageNumber = query.Page.Value;
        int pageSize = query.Size.Value;
        int totalPages = (int)Math.Ceiling(totalCommentsCount / (double)query.Size.Value);

        PageInfo pageInfo = new PageInfo
        {
            TotalElements = totalCommentsCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages
        };

        return new CommentsPageResponse
        {
            Comments = filteredCommentsList,
            PageInfo = pageInfo
        };
    }
}
