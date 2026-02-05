using Microsoft.AspNetCore.Mvc;

namespace Taller_Challenge_Backend.Pricing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PricingController : ControllerBase
    {
        [HttpGet("calculate")]
        public IActionResult Calculate([FromQuery] decimal subtotal)
        {
            var tax = subtotal * 0.15m;

            var discount = subtotal > 500 ? subtotal * 0.10m : 0;

            return Ok(new
            {
                TaxAmount = tax,
                DiscountAmount = discount,
                Total = subtotal + tax - discount
            });
        }
    }
}
