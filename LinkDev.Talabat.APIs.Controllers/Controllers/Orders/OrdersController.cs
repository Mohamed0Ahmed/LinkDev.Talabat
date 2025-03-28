using LinkDev.Talabat.APIs.Controllers.Controllers.Base;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Orders;
using LinkDev.Talabat.Core.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Orders
{
   
       public class OrdersController(IServiceManager serviceManager) : BaseApiController
    {

        [HttpPost] // Post  :   /api/orders
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder([FromBody]OrderToCreateDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var result = await serviceManager.OrderService.CreateOrderAsync(buyerEmail!, orderDto);
           


            return Ok(result);
        }

        [HttpGet] // GET: /api/orders
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetOrdersForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManager.OrderService.GetOrderForUserAsync(BuyerEmail!);
            return Ok(result);
        }


        [HttpGet("{id}")] // GET: /api/orders/id

        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetOrdersById(int id)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManager.OrderService.GetOrderByIdAsync(BuyerEmail!, id);
            return Ok(result);
        }


        [HttpGet("deliveryMethods")] // GET: /api/orders/deliveryMethods
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethod()
        {
            var result = await serviceManager.OrderService.GetDeliveryMethodAsync();
            return Ok(result);
        }
    }
}
