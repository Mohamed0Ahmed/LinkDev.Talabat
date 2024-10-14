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
            AddIncludes();
        }

   

        public ProductLoadingBrandAndCategorySpecifications(int id) :base(id) 
        {
            AddIncludes();
        }

        #region Helper

        private void AddIncludes()
        {
            Includes.Add(P => P.Brand!);
            Includes.Add(P => P.Category!);
        }

        #endregion

    }
}
