using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Contracts;
using System.Linq.Expressions;

namespace LinkDev.Talabat.Core.Domain.Specifications
{
    public abstract class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {

        public Expression<Func<TEntity, bool>>? Criteria { get; set; }
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new();  // Initialize with empty list 
        public Expression<Func<TEntity, object>>? OrderBy { get; set; }
        public Expression<Func<TEntity, object>>? OrderByDesc { get; set; }
        public int Skip { get ; set ; }
        public int Take { get ; set ; }
        public bool IsPaginationEnable { get ; set ; }



        #region CTOR

        public BaseSpecifications(Expression<Func<TEntity, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
        }

        public BaseSpecifications(TKey id)
        {
            Criteria = (E => E.Id.Equals(id));

        }

        #endregion




        #region Methods

        private protected virtual void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }


        private protected virtual void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDescExpression)
        {

            OrderByDesc = orderByDescExpression;
        }

        private protected virtual void AddIncludes()
        {
    
        }

        private protected void ApplyPagination(int skip , int take)
        {
            IsPaginationEnable = true;
            Skip = skip;
            Take = take;
        }

        #endregion


    }
}

