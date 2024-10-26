using LinkDev.Talabat.Core.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Identities.Configurations
{
    internal class AddressConfigurations : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {

            builder.ToTable("Addresses");


            builder.Property(a=>a.Id).ValueGeneratedOnAdd();
            builder.Property(a => a.FirstName).HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(a => a.LastName).HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(a => a.Street).HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(a => a.City).HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(a => a.Country).HasColumnType("nvarchar").HasMaxLength(50);

        }
    }
}
