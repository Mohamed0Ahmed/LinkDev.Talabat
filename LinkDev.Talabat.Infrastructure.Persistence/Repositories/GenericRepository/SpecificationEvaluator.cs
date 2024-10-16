using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Repositories.GenericRepository
{
    internal static class SpecificationEvaluator<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity, TKey> spec)
        {
            var query = inputQuery;                      // _dbContext.Set<TEntity>()

            if (spec.Criteria is not null)                // P => P.Id.Equals(1)
                query = query.Where(spec.Criteria);      // _dbContext.Set<TEntity>().where(P => P.Id.Equals(1))




            if (spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);

            else if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);


            if (spec.IsPaginationEnable)
                query = query.Skip(spec.Skip).Take(spec.Take);

                query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));


            return query;
        }
    }
}
