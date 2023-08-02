namespace SharedEventContracts;

public interface IFetchCommentsEvent
{
    string VideoId { get; set; }
    string PageToken { get; set; }
}