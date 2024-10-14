using LinkDev.Talabat.Core.Domain.Common;

namespace LinkDev.Talabat.Core.Domain.Contracts.Persistence
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
                           where TEntity : BaseAuditableEntity<TKey>
                           where TKey : IEquatable<TKey>;


        Task<int> CompleteAsync();
    }
}
