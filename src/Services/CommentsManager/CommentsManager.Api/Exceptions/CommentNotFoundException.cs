namespace CommentsManager.Api.Exceptions;

public class CommentNotFoundException : NotFoundException
{
    public CommentNotFoundException(string videoId) : base(
        $"Comments for videoId: {videoId} not found.")
    {
    }
}