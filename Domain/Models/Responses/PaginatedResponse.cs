namespace Domain.Models.Responses;

public class PaginatedResponse<T>
{
    public PaginatedResponse() { }

    public PaginatedResponse(int pageNumber, int pageSize, string? nextPage, string? previousPage, int totalPages, int totalData, T[] data)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        NextPage = nextPage;
        PreviousPage = previousPage;
        TotalPages = totalPages;
        TotalData = totalData;
        Data = data;
    }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? NextPage { get; set; }
    public string? PreviousPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalData { get; set; }
    public T[] Data { get; set; }
}