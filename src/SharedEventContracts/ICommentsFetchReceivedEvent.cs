namespace SharedEventContracts;

public interface ICommentsFetchReceivedEvent
{
    string VideoId { get; }
    string PageToken { get; }
}