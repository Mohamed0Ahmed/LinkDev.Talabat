using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Specifications.Products
{
    public class ProductWithBrandCategoryAndSortSpecification : BaseSpecifications<Product , int>
    {
        public ProductWithBrandCategoryAndSortSpecification(string? sort) : base()
        {
            
            AddIncludes();
            AddOrderBy(p => p.Name);

            if (!string.IsNullOrEmpty(sort))
            {
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
            }    
        }

   

        public ProductWithBrandCategoryAndSortSpecification(int id) :base(id) 
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
