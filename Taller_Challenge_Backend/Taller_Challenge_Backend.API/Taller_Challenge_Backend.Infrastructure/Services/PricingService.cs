using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net.Http.Json;
using Taller_Challenge_Backend.Domain.Interfaces;
using Taller_Challenge_Backend.Domain.Models.Responses;

namespace Taller_Challenge_Backend.Infrastructure.Services
{
    public class PricingService : IPricingService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PricingService> _logger;

        public PricingService(HttpClient httpClient, ILogger<PricingService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<PricingResponse> CalculatePricingAsync(decimal subtotalAmount, CancellationToken cancellationToken = default)
        {
            // This try-catch is used to intercept any kind of exceptions before they reach the Middleware global handler.
            // This ensures the Fallback logic can provide a valid response instead of returning an error to the client.
            try
            {
                _logger.LogInformation("Calling legacy pricing service for subtotal: {Subtotal}", subtotalAmount);

                var encodedSubtotal = Uri.EscapeDataString(subtotalAmount.ToString(CultureInfo.InvariantCulture));
                var url = $"api/Pricing/calculate?subtotal={encodedSubtotal}";

                var response = await _httpClient.GetAsync(url, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<PricingResponse>(cancellationToken)
                           ?? throw new InvalidOperationException("Empty response");
                }

                _logger.LogWarning("Service failed after retries. Switching to fallback.");
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                _logger.LogError(ex, "Pricing service unavailable.");
            }

            // Simple Retry 
            return await CalculateFallbackPricing(subtotalAmount);
        }

        private Task<PricingResponse> CalculateFallbackPricing(decimal subtotal)
        {
            // Backup logic if service fails
            var taxAmount = subtotal * 0.05m;
            var discountAmount = subtotal > 1000 ? subtotal * 0.05m : 0;

            return Task.FromResult(new PricingResponse(
                TaxAmount: taxAmount,
                DiscountAmount: discountAmount,
                Total: subtotal + taxAmount - discountAmount
            ));
        }
    }
}
