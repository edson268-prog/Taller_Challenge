using Taller_Challenge_Backend.Domain.Entities;
using Taller_Challenge_Backend.Domain.Models.Responses;

namespace Taller_Challenge_Backend.API.Extensions
{
    public static class OrderMappingExtensions
    {
        public static OrderResponse ToResponse(this Order order)
        {
            if (order == null) return null;

            return new OrderResponse(
                order.Id,
                order.CustomerName,
                order.VehiclePlate,
                order.Status.ToString(),
                order.CreatedAt,
                order.Subtotal,
                order.TaxAmount,
                order.DiscountAmount,
                order.TotalAmount,
                order.Items?.Select(i => i.ToResponse()).ToList() ?? new List<OrderItemResponse>()
            );
        }

        public static OrderItemResponse ToResponse(this OrderItem item)
        {
            if (item == null) return null;

            return new OrderItemResponse(
                item.Id,
                item.Description,
                item.Quantity,
                item.UnitPrice,
                item.Quantity * item.UnitPrice
            );
        }

        public static IEnumerable<OrderResponse> ToResponse(this IEnumerable<Order> orders)
        {
            return orders?.Select(o => o.ToResponse()) ?? Enumerable.Empty<OrderResponse>();
        }
    }
}
