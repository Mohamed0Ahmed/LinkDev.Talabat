using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Specifications.Products
{
    public class ProductWithFiltrationForCountSpecifications : BaseSpecifications<Product, int>
    {

        public ProductWithFiltrationForCountSpecifications( int? brandId, int? categoryId)
             : base(p => (!brandId.HasValue || p.BrandId == brandId.Value) &&
                       (!categoryId.HasValue || p.CategoryId == categoryId.Value))
        {

        }

    }
}
