using CommentsManager.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentsManager.Api.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<Comment> SearchByText(this IQueryable<Comment> query, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return query;

        return query.Where(c => EF.Functions.ToTsVector("english", c.TextDisplay) 
            .Matches(EF.Functions.ToTsQuery("english", $"\"{searchTerm}\"")));
    }

    public static IQueryable<Comment> SearchByAuthor(this IQueryable<Comment> query, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return query;

        return query.Where(c => EF.Functions.ToTsVector("english", c.AuthorDisplayName) 
            .Matches(EF.Functions.ToTsQuery("english", $"\"{searchTerm}\"")));
    }

}
