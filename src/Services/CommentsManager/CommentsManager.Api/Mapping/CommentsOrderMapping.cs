using System.Linq.Expressions;
using CommentsManager.Api.Enums;
using CommentsManager.Api.Models;

namespace CommentsManager.Api.Mapping;

public static class CommentsOrderMapping
{

    public static readonly Dictionary<OrderBy, Expression<Func<Comment, object>>> Criteria =
        new Dictionary<OrderBy, Expression<Func<Comment, object>>>
        {
            {OrderBy.PublishedDate, c => c.PublishedAt},
            {OrderBy.Likes, c => c.LikeCount},
            {OrderBy.Replies, c => c.TotalReplyCount},
            {OrderBy.Author, c => c.AuthorDisplayName}
        };
}
