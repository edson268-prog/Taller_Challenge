using Microsoft.AspNetCore.Mvc;
using Taller_Challenge_Backend.Domain.Interfaces;

namespace Taller_Challenge_Backend.API.Orders.Commands
{
    public class CalculateOrderPriceCommand
    {
        public static async Task<IResult> ExecuteCommand([FromRoute] Guid id, [FromServices] IOrderRepository orderRepository, [FromServices] IPricingService pricingService)
        {
            var order = await orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return Results.NotFound(new { Message = $"Order with ID {id} not found" });
            }

            // If the subtotal has not been calculated
            if (order.Subtotal == 0)
            {
                order.CalculateSubtotal();
            }

            // Call to Pricing Service (legacy)
            var pricingResult = await pricingService.CalculatePricingAsync(order.Subtotal);

            order.ApplyPricing(pricingResult.TaxAmount, pricingResult.DiscountAmount);

            await orderRepository.UpdateAsync(order);
            await orderRepository.SaveChangesAsync();

            var response = new
            {
                OrderId = order.Id,
                Subtotal = order.Subtotal,
                Taxes = pricingResult.TaxAmount,
                Discounts = pricingResult.DiscountAmount,
                TotalFinal = pricingResult.Total
            };

            return Results.Ok(response);
        }
    }
}
