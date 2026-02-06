using Microsoft.AspNetCore.Mvc;
using Taller_Challenge_Backend.API.Extensions;
using Taller_Challenge_Backend.Domain.Enums;
using Taller_Challenge_Backend.Domain.Interfaces;
using Taller_Challenge_Backend.Domain.Models.Requests;

namespace Taller_Challenge_Backend.API.Orders.Queries
{
    public static class GetAllOrdersQuery
    {
        public static async Task<IResult> ExecuteQuery([FromServices] IOrderRepository orderRepository,[AsParameters] GetOrdersRequest query)
        {
            if (query.Page < 1)
                return Results.BadRequest("Page must be greater than 0");

            OrderStatus? statusEnum = null;
            if (!string.IsNullOrEmpty(query.Status) && Enum.TryParse<OrderStatus>(query.Status, true, out var parsedStatus))
            {
                statusEnum = parsedStatus;
            }

            var orders = await orderRepository.GetFilteredOrdersAsync(
                statusEnum,
                query.Page,
                query.PageSize,
                query.SortOrder);

            var response = orders.ToResponse();

            return Results.Ok(response);
        }
    }
}
