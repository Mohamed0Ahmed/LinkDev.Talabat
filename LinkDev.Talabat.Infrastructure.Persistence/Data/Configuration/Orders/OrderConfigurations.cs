using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Configuration.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Configuration.Orders
{
    public class OrderConfigurations : BaseAuditableEntityConfigurations<Order, int>
    {

        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);


            builder.OwnsOne(O => O.ShippingAddress, Sh => Sh.WithOwner());

            builder.Property(O => O.Status)
                .HasConversion(
                (orderStatus) => orderStatus.ToString(),
                (orderStatus) => (OrderStatus)Enum.Parse(typeof(OrderStatus), orderStatus)
                );


            builder.Property(order => order.SubTotal).HasColumnType("decimal(8,2)");


            builder.HasOne(order => order.DeliveryMethod)
                   .WithMany()
                   .HasForeignKey(order=>order.DeliveryMethodId)
                   .OnDelete(DeleteBehavior.SetNull);
            
            builder.HasMany(order => order.Items)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
