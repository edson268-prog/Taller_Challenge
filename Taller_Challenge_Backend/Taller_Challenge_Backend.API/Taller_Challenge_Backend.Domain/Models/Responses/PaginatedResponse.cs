namespace Taller_Challenge_Backend.Domain.Models.Responses
{
    public record PaginatedResponse<T>(
        IEnumerable<T> Items,
        int Page,
        int PageSize,
        int TotalCount,
        int TotalPages
    );
}
