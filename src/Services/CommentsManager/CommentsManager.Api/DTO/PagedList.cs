namespace CommentsManager.Api.DTO;

public class PagedList<T>
{
    public List<T> Items { get; set; }
    public PageInfo PageInfo { get; set; }

    public PagedList(List<T> items, int totalCount, int pageNumber, int pageSize)
    {
        PageInfo = new PageInfo()
        {
            TotalElements = totalCount,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
        
        Items = items;
    }
}


