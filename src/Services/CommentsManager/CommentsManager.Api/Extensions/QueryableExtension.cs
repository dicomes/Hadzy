using CommentsManager.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentsManager.Api.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<Comment> SearchByText(this IQueryable<Comment> query, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return query;

        var tsQuery = EF.Functions.ToTsQuery("simple", searchTerm);
        return query.Where(c => EF.Functions.ToTsVector("simple", c.TextDisplaySearchVector).Matches(tsQuery));
    }

    public static IQueryable<Comment> SearchByAuthor(this IQueryable<Comment> query, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return query;

        var tsQuery = EF.Functions.ToTsQuery("simple", searchTerm);
        return query.Where(c => EF.Functions.ToTsVector("simple", c.AuthorDisplayNameSearchVector).Matches(tsQuery));
    }
}

