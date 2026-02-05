namespace Taller_Challenge_Backend.Domain.Models.Requests
{
    public record CreateOrderRequest(
        string CustomerName,
        string VehiclePlate,
        List<OrderItemRequest> Items);

    public record OrderItemRequest(
        string Description,
        int Quantity,
        decimal UnitPrice);
}
