using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Configuration.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Configuration.Orders
{
    public class OrderItemConfigurations : BaseAuditableEntityConfigurations<OrderItem , int>
    {

        public override void Configure(EntityTypeBuilder<OrderItem> entity)
        {
            base.Configure(entity);

            entity.OwnsOne(item => item.Product, product => product.WithOwner());

            entity.Property(item => item.Price).HasColumnType("decimal(8,2)");
        }
    }
}
