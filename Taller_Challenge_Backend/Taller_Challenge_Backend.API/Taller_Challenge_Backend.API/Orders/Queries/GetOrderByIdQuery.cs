using Microsoft.AspNetCore.Mvc;
using Taller_Challenge_Backend.API.Extensions;
using Taller_Challenge_Backend.Domain.Interfaces;

namespace Taller_Challenge_Backend.API.Orders.Queries
{
    public static class GetOrderByIdQuery
    {
        public static async Task<IResult> ExecuteQuery(
        [FromRoute] Guid id,
        [FromServices] IOrderRepository orderRepository)
        {
            var order = await orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return Results.NotFound(new { Message = $"Order with ID {id} not found" });
            }

            var response = order.ToResponse();
            return Results.Ok(response);
        }
    }
}
