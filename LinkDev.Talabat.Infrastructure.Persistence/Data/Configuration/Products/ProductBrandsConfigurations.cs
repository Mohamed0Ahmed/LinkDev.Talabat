using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Configuration.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Configuration.Products
{
    public class ProductBrandsConfigurations : BaseEntityConfigurations<ProductBrand,int>
    {

        public override void Configure(EntityTypeBuilder<ProductBrand> brand)
        {
            base.Configure(brand);

            brand.ToTable("Brands");
            brand.Property(p => p.Name).IsRequired();
        }
    }
}
