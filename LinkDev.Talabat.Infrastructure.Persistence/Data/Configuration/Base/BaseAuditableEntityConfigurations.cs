using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Configuration.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Configuration.Base
{
    public class BaseAuditableEntityConfigurations <TEntity , TKey> : BaseEntityConfigurations<TEntity , TKey>
        where TEntity : BaseAuditableEntity<TKey > where TKey : IEquatable  <TKey>
    {
        public override void Configure(EntityTypeBuilder<TEntity> entity)
        {


            entity.Property(e => e.CreatedBy).IsRequired();
            entity.Property(e => e.CreatedOn).IsRequired();
            entity.Property(e => e.LastModifiedBy).IsRequired();
            entity.Property(e => e.LastModifiedOn).IsRequired();



        }
    }
}

