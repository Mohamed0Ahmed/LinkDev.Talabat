using LinkDev.Talabat.APIs.Controllers.Controllers.Base;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Payment
{

    [ApiController]
    public class PaymentController(IPaymentService paymentService) : BaseApiController
    {

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var result = await paymentService.CreateOrUpdatePaymentIntent(basketId);
            return Ok(result);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> HandleWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var sigHeader = Request.Headers["Stripe-Signature"];
            await paymentService.UpdateOrderPaymentStatus(json, sigHeader!);



            return Ok();
        }
    }
}

