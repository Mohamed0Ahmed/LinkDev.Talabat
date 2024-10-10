using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Infrastructure.Common.Abstractions;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using System.Collections.Concurrent;

namespace LinkDev.Talabat.Infrastructure.Persistence.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {


        #region Services


        private readonly StoreContext _dbContext;
        private readonly ConcurrentDictionary<string, object> _repositories; 

        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new ConcurrentDictionary<string, object>();
        } 


        #endregion





        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
                                 where TEntity : BaseAuditableEntity<TKey>
                                 where TKey : IEquatable<TKey>
        {
         

            return (IGenericRepository<TEntity, TKey>) _repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity , TKey>(_dbContext));

        }



        public async Task<int> CompleteAsync() => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();   

       
    }
}
