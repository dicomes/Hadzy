using CommentsManager.Api.Contracts.Utilities;

namespace CommentsManager.Api.Utilities;

public class SanitizationService : ISanitizationService
{
    public string? SanitizeSearchTerm(string? input) 
    {
        if (string.IsNullOrEmpty(input))
            return input;
        
        var firstWord = input.Split(' ').FirstOrDefault();

        // Remove punctuation.
        var cleaned = new string(firstWord
            .Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
            .ToArray());

        // Convert to lowercase for consistency
        return cleaned?.ToLowerInvariant();
    }
}

