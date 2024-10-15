using LinkDev.Talabat.APIs.Controllers.Controllers.Base;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Products;
using LinkDev.Talabat.Core.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Products
{
    public class ProductsController(IServiceManager serviceManager) : BaseApiController
    {


        [HttpGet]                    // GET : /api/products
        public async Task<ActionResult<IEnumerable<ProductDisplayDto>>> GetProducts(string? sort , int? brandId , int? categoryId)
        {
            var products = await serviceManager.ProductService.GetProductsAsync(sort , brandId , categoryId);
            return Ok(products);
        }


        [HttpGet("{id:int}")]        // GET : /api/products/1
        public async Task<ActionResult<ProductDisplayDto>> GetProduct (int id )
        {
            var product = await serviceManager.ProductService.GetProductAsync(id);
            if (product == null) 
                return NotFound( new { StatusCode = 404 , message = "not found."});

            return Ok(product);
        }


        [HttpGet("brands")]          // GET : /api/products/brands
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetBrands()
        {
            var brands = await serviceManager.ProductService.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("categories")]      // GET : /api/products/categories
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await serviceManager.ProductService.GetCategoriesAsync();
            return Ok(categories);
        }

    }
}
