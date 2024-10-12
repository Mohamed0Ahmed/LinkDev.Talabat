using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Products;
using LinkDev.Talabat.Core.Application.Abstraction.Interfaces;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Infrastructure.Common.Abstractions;

namespace LinkDev.Talabat.Core.Application.Services.Products
{
    internal class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<ProductDisplayDto>> GetProductsAsync()
        {
            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync();

            var productDisplayDto = mapper.Map<IEnumerable<ProductDisplayDto>>(products);

            return productDisplayDto;

        }

        public async Task<ProductDisplayDto> GetProductAsync(int id)
        {

            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(id);
            var productDisplayDto = mapper.Map<ProductDisplayDto>(product);

            return productDisplayDto;
        }

        public async Task<IEnumerable<BrandDto>> GetBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();

            var brandsDto = mapper.Map<IEnumerable<BrandDto>>(brands);

            return brandsDto;

        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await unitOfWork.GetRepository<ProductCategory, int>().GetAllAsync();

            var categoriesDto = mapper.Map<IEnumerable<CategoryDto>>(categories);

            return categoriesDto;
        }



    }
}
