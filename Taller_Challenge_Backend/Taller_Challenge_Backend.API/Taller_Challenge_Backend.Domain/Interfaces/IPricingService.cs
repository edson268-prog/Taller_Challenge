using Taller_Challenge_Backend.Domain.Models.Responses;

namespace Taller_Challenge_Backend.Domain.Interfaces
{
    public interface IPricingService
    {
        Task<PricingResponse> CalculatePricingAsync(decimal subtotal, CancellationToken cancellationToken = default);
    }
}
