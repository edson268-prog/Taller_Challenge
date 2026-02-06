using Microsoft.AspNetCore.Mvc;

namespace Taller_Challenge_Backend.Domain.Models.Requests
{
    public record GetOrdersRequest(
        [FromQuery] string? Status = null,
        [FromQuery] int Page = 1,
        [FromQuery] int PageSize = 5,
        [FromQuery] string SortOrder = "desc"
    );
}
