namespace CommentsManager.Api.Contracts.Utilities;

public interface ISanitizationService
{
    string? SanitizeSearchTerm(string? input);
}