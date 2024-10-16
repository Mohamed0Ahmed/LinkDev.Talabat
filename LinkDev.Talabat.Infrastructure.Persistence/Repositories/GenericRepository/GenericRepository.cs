using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Contracts;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.Talabat.Infrastructure.Persistence.Repositories.GenericRepository
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
        {

            ///if (typeof(TEntity) == typeof(Product))
            ///{
            ///    return withTracking ?
            ///           (IEnumerable<TEntity>)await _dbContext.Set<Product>().Include(p => p.Brand).Include(p => p.Category).ToListAsync() :
            ///           (IEnumerable<TEntity>)await _dbContext.Set<Product>().Include(p => p.Brand).Include(p => p.Category).AsNoTracking().ToListAsync();
            ///}


            return withTracking ?
            await _dbContext.Set<TEntity>().ToListAsync() :
            await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();

        }

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> spec, bool withTracking = false)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }


        public async Task<TEntity?> GetAsync(TKey id)
        {
            ///if (typeof(TEntity) == typeof(Product))
            ///    return await _dbContext.Set<Product>().Where(p => p.Id.Equals(id)).Include(p => p.Brand).Include(p => p.Category).FirstOrDefaultAsync() as TEntity;

            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity?> GetWithSpecAsync(ISpecifications<TEntity, TKey> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISpecifications<TEntity, TKey> spec)
        {
            
            return await ApplySpecifications(spec).CountAsync();

        }


        public async Task AddAsync(TEntity entity) => await _dbContext.AddAsync(entity);

        public void Delete(TEntity entity) => _dbContext.Remove(entity);

        public void Update(TEntity entity) => _dbContext.Update(entity);



        #region Helpers Method


        private IQueryable<TEntity> ApplySpecifications(ISpecifications<TEntity, TKey> spec)
        {
            return SpecificationEvaluator<TEntity, TKey>.GetQuery(_dbContext.Set<TEntity>(), spec);
        }

     


        #endregion


    }
}
