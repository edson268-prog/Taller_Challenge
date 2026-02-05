namespace Taller_Challenge_Backend.Domain.Models.Responses
{
    public record PricingResponse(
        decimal TaxAmount,
        decimal DiscountAmount,
        decimal Total);
}
