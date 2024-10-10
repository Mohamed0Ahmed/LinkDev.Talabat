using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Infrastructure.Abstractions;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.Talabat.Infrastructure.Persistence.Repositories
{
    internal class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
        where TEntity : BaseAuditableEntity<TKey>
        where TKey : IEquatable<TKey>
    {


        #region Services

        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion






        public async Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false)
            => withTracking ? await _dbContext.Set<TEntity>().ToListAsync() : await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();



        public async Task<TEntity?> GetAsync(TKey id) => await _dbContext.Set<TEntity>().FindAsync(id);


        public async Task AddAsync(TEntity entity)=> await _dbContext.AddAsync(entity);

        public void Delete(TEntity entity) => _dbContext.Remove(entity);

        public void Update(TEntity entity)=> _dbContext.Update(entity);
    }
}
