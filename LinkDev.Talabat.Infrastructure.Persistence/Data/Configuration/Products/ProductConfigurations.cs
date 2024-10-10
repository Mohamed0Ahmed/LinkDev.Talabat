using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Configuration.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Configuration.Products
{
    public class ProductConfigurations : BaseAuditableEntityConfigurations<Product,int>
    {
        public override void Configure(EntityTypeBuilder<Product> product)
        {
            base.Configure(product);

            product.Property(p => p.Name).IsRequired().HasMaxLength(100);
            product.Property(p => p.Description).IsRequired();
            product.Property(p => p.Price).IsRequired().HasColumnType("decimal(9,2)");


            product.HasOne(p => p.Brand)
                   .WithMany()
                   .HasForeignKey(p => p.BrandId)
                   .OnDelete(DeleteBehavior.SetNull); 
            
            product.HasOne(p => p.Category)
                   .WithMany()
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.SetNull);



        }
    }
}
