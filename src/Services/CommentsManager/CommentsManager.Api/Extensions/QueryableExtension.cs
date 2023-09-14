using CommentsManager.Api.Models;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace CommentsManager.Api.Extensions;

public static class CommentSearchExtensions
{
    public static IQueryable<Comment> SearchByText(this IQueryable<Comment> query, string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return query;
        
        var tsQuery = NpgsqlTsQuery.Parse($"{searchTerm.ToLower()}:*");
        return query.Where(c => c.TextDisplaySearchVector.Matches(tsQuery));
    }

    public static IQueryable<Comment> SearchByAuthor(this IQueryable<Comment> query, string author)
    {
        if (string.IsNullOrEmpty(author))
            return query;
        
        var tsQuery = NpgsqlTsQuery.Parse($"{author.ToLower()}:*");
        return query.Where(c => c.AuthorDisplayNameSearchVector.Matches(tsQuery));
    }
}

