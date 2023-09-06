namespace CommentsManager.Api.DTO;

public class ApiResponse<T>
{
    public List<string> ErrorMessages { get; set; } = new List<string>();
    public T? Result { get; set; }
}