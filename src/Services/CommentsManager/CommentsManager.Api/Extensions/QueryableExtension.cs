using System.Linq.Expressions;
using CommentsManager.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentsManager.Api.Extensions;

public static class CommentSearchExtensions
{
    public static IQueryable<Comment> SearchByText(this IQueryable<Comment> query, string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return query;

        return query.Where(c => c.TextDisplaySearchVector.Matches(searchTerm));
    }

    public static IQueryable<Comment> SearchByAuthor(this IQueryable<Comment> query, string author)
    {
        if (string.IsNullOrEmpty(author))
            return query;

        return query.Where(c => c.AuthorDisplayNameSearchVector.Matches(author));
    }
}

