namespace SharedEventContracts;

public interface ICommentsFetchReceivedEvent
{
    string VideoId { get; set; }
    string PageToken { get; set; }
    
}