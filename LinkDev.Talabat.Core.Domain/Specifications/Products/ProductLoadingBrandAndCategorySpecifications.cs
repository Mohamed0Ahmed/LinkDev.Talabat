using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Specifications.Products
{
    public class ProductLoadingBrandAndCategorySpecifications : BaseSpecifications<Product , int>
    {
        public ProductLoadingBrandAndCategorySpecifications() : base()
        {
            Includes.Add(P => P.Brand!);
            Includes.Add(P => P.Category!);
        }

    }
}
