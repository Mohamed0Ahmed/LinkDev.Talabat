using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Configuration.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Configuration.Orders
{
    public class DeliveryMethodConfigurations :BaseEntityConfigurations<DeliveryMethod, int>
    {
        public override void Configure(EntityTypeBuilder<DeliveryMethod> entity)
        {
            base.Configure(entity);

            entity.Property(method => method.Cost).HasColumnType("decimal (8,2)");
        }
    }
}
