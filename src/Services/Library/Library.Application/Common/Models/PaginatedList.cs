namespace Library.Application.Common.Models
{
    // Record class for immutable, generic pagination response
    public record PaginatedList<T>(
        List<T> Items,
        int PageNumber,
        int TotalPages,
        int TotalCount,
        int PageSize
    );
}
