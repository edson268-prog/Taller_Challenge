using Microsoft.AspNetCore.Mvc;
using Taller_Challenge_Backend.API.Extensions;
using Taller_Challenge_Backend.Domain.Enums;
using Taller_Challenge_Backend.Domain.Interfaces;
using Taller_Challenge_Backend.Domain.Models.Requests;

namespace Taller_Challenge_Backend.API.Orders.Commands
{
    public static class UpdateOrderStatusCommand
    {
        public static async Task<IResult> ExecuteCommand([FromRoute] Guid id, [FromBody] UpdateOrderStatusRequest request, [FromServices] IOrderRepository orderRepository)
        {
            var order = await orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return Results.NotFound(new { Message = $"Order with ID {id} not found" });
            }

            if (!Enum.TryParse<OrderStatus>(request.Status, true, out var newStatus))
            {
                return Results.BadRequest(new { Message = $"Invalid status value: {request.Status}" });
            }

            order.UpdateStatus(newStatus);

            await orderRepository.UpdateAsync(order);
            await orderRepository.SaveChangesAsync();

            var response = order.ToResponse();
            return Results.Ok(response);
        }
    }
}
