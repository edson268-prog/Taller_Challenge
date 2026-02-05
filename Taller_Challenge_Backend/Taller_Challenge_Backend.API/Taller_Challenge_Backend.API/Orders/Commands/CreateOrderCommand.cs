using Microsoft.AspNetCore.Mvc;
using Taller_Challenge_Backend.API.Extensions;
using Taller_Challenge_Backend.Domain.Entities;
using Taller_Challenge_Backend.Domain.Interfaces;
using Taller_Challenge_Backend.Domain.Models.Requests;

namespace Taller_Challenge_Backend.API.Orders.Commands
{
    public static class CreateOrderCommand
    {
        public static async Task<IResult> ExecuteCommand([FromBody] CreateOrderRequest request, [FromServices] IOrderRepository orderRepository)
        {
            var orderItems = request.Items.Select(item => OrderItem.Create(item.Description, item.Quantity, item.UnitPrice)).ToList();

            var order = Order.Create(request.CustomerName, request.VehiclePlate, orderItems);

            await orderRepository.AddAsync(order);
            await orderRepository.SaveChangesAsync();

            var response = order.ToResponse();
            return Results.Created($"/api/v1/orders/{order.Id}", response);
        }
    }
}
