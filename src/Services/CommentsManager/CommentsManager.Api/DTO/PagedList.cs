namespace CommentsManager.Api.DTO;

public class PagedList<T> : List<T>
{
    public PageInfo PageInfo { get; set; }
    public IEnumerable<T> Comments { get; set; }

    public PagedList(List<T> items, int totalCount, int pageNumber, int pageSize)
    {
        PageInfo = new PageInfo()
        {
            TotalElements = totalCount,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int) Math.Ceiling(totalCount / (double) pageSize)
        };
        
        AddRange(items);
    }
    
    public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source.ToList();
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}

