using Microsoft.AspNetCore.Mvc;
using Taller_Challenge_Backend.Domain.Entities;
using Taller_Challenge_Backend.Domain.Enums;
using Taller_Challenge_Backend.Domain.Interfaces;
using Taller_Challenge_Backend.API.Extensions;

namespace Taller_Challenge_Backend.API.Orders.Queries
{
    public static class GetAllOrdersQuery
    {
        public static async Task<IResult> ExecuteQuery([FromServices] IOrderRepository orderRepository, [FromQuery] string? status = null)
        {
            IEnumerable<Order> orders;

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<OrderStatus>(status, true, out var statusEnum))
            {
                orders = await orderRepository.GetByStatusAsync(statusEnum);
            }
            else
            {
                orders = await orderRepository.GetAllAsync();
            }

            var response = orders.ToResponse();
            return Results.Ok(response);
        }
    }
}
