using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstraction.Interfaces
{
    public interface IProductService
    {

        Task<IEnumerable<ProductDisplayDto>> GetProductsAsync(string? sort, int? brandId, int? categoryId);
        Task<ProductDisplayDto> GetProductAsync(int id);
        Task<IEnumerable<BrandDto>> GetBrandsAsync();
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    }
}
