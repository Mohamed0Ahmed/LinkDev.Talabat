using LinkDev.Talabat.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Abstractions
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
                           where TEntity : BaseAuditableEntity<TKey>
                           where TKey : IEquatable<TKey>;


        Task<int> CompleteAsync();
    }
}
