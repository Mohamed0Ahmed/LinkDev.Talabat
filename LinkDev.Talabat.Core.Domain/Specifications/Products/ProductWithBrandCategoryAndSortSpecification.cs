using LinkDev.Talabat.Core.Domain.Entities.Products;

namespace LinkDev.Talabat.Core.Domain.Specifications.Products
{
    public class ProductWithBrandCategoryAndSortSpecification : BaseSpecifications<Product, int>
    {
        public ProductWithBrandCategoryAndSortSpecification(string? sort, int? brandId, int? categoryId, int pageIndex, int pageSize, string? Search)
            : base(p => (string.IsNullOrEmpty(Search) ||p.NormalizedName.Contains(Search) ) &&
                        (!brandId.HasValue || p.BrandId == brandId.Value) &&
                        (!categoryId.HasValue || p.CategoryId == categoryId.Value))
        {

            AddIncludes();
            switch (sort)
            {
                case "nameDesc":
                    AddOrderByDesc(p => p.Name);
                    break;

                case "price":
                    AddOrderBy(p => p.Price);
                    break;

                case "priceDesc":
                    AddOrderByDesc(p => p.Price);
                    break;

                default:
                    AddOrderBy(p => p.Name);
                    break;
            }


            ApplyPagination(pageSize * (pageIndex - 1), pageSize);
        }



        public ProductWithBrandCategoryAndSortSpecification(int id)
            : base(id)
        {
            AddIncludes();
        }


        private protected override void AddIncludes()
        {
            base.AddIncludes();

            Includes.Add(p => p.Brand!);
            Includes.Add(p => p.Category!);
        }

    }
}
