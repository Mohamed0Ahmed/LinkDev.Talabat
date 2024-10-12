using LinkDev.Talabat.APIs.Controllers.Controllers.Base;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Products;
using LinkDev.Talabat.Core.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Products
{
    public class ProductsController(IServiceManager serviceManager) : BaseApiController
    {


        [HttpGet] // GET : api/
        public async Task<ActionResult<IEnumerable<ProductDisplayDto>>> GetProducts()
        {
            var products = await serviceManager.ProductService.GetProductsAsync();
            return Ok(products);
        }


    }
}
