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
        IGenericRepository<TEntity, TKey> GetGenericRepository<TEntity, TKey>()
                           where TEntity : BaseEntity<TKey>
                           where TKey : IEquatable<TKey>;


        Task<int> CompleteAsync();
    }
}
