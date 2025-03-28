using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Configuration.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Configuration.Products
{
    public class ProductCategoryConfigurations : BaseAuditableEntityConfigurations<ProductCategory,int>
    {

        public override void Configure(EntityTypeBuilder<ProductCategory> category)
        {
            base.Configure(category);

            category.ToTable("Categories");

            category.Property(c => c.Name).IsRequired();
        }

    }
}
