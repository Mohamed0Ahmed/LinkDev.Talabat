using LinkDev.Talabat.APIs.Controllers.Controllers.Base;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Products;
using LinkDev.Talabat.Core.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Products
{
    public class ProductsController(IServiceManager serviceManager) : BaseApiController
    {


        [HttpGet] // GET : /api/products
        public async Task<ActionResult<IEnumerable<ProductDisplayDto>>> GetProducts()
        {
            var products = await serviceManager.ProductService.GetProductsAsync();
            return Ok(products);
        }


        [HttpGet("{id:int}")] 
        public async Task<ActionResult<ProductDisplayDto>> GetProduct (int id )
        {
            var product = await serviceManager.ProductService.GetProductAsync(id);
            if (product == null) 
                return NotFound( new { StatusCode = 404 , message = "not found."});

            return Ok(product);
        }

    }
}
