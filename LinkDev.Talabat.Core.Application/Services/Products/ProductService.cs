using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Common;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Products;
using LinkDev.Talabat.Core.Application.Abstraction.Interfaces;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Core.Domain.Specifications.Products;

namespace LinkDev.Talabat.Core.Application.Services.Products
{
    internal class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<Pagination<ProductDisplayDto>> GetProductsAsync(ProductSpecParams specParams)
        {

            var spec = new ProductWithBrandCategoryAndSortSpecification(specParams.Sort, specParams.BrandId, specParams.CategoryId, specParams.PageIndex, specParams.PageSize , specParams.Search);

            var products = await unitOfWork.GetRepository<Product, int>().GetAllWithSpecAsync(spec);

            var productDisplayDto = mapper.Map<IEnumerable<ProductDisplayDto>>(products);

            var countSpec = new ProductWithFiltrationForCountSpecifications(specParams.BrandId, specParams.CategoryId, specParams.Search);
            var count = await unitOfWork.GetRepository<Product, int>().GetCountAsync(countSpec);

            return new Pagination<ProductDisplayDto>(specParams.PageSize, specParams.PageIndex , count) { Data = productDisplayDto };

        }

        public async Task<ProductDisplayDto> GetProductAsync(int id)
        {

            var spec = new ProductWithBrandCategoryAndSortSpecification(id);

            var product = await unitOfWork.GetRepository<Product, int>().GetWithSpecAsync(spec);
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
