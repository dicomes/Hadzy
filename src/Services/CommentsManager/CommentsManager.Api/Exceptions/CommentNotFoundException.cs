namespace CommentsManager.Api.Exceptions;

public class CommentNotFoundException : NotFoundException
{
    public CommentNotFoundException(string videoId) : base(
        $"Comment for videoId: {videoId} not found.")
    {
    }
}