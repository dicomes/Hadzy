using EFContracts;

namespace CommentsStorage.Worker.Models;

public class Video : IVideo
{
    public string Id { get; set; }
    public string? FirstComment { get; set; }
} 