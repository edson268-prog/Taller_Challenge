namespace Taller_Challenge_Backend.Domain.Models.Responses
{
    public record OrderResponse(
        Guid Id,
        string CustomerName,
        string VehiclePlate,
        string Status,
        DateTime CreatedAt,
        decimal Subtotal,
        decimal TaxAmount,
        decimal DiscountAmount,
        decimal TotalAmount,
        List<OrderItemResponse> Items);

    public record OrderItemResponse(
        Guid Id,
        string Description,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice);
}
