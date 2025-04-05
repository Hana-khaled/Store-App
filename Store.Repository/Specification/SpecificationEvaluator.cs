using Microsoft.EntityFrameworkCore;
using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification
{
    public class SpecificationEvaluator<TEntity, TKey> where TEntity:BaseEntity<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> baseQuery, ISpecification<TEntity>specs)
        {
            var query = baseQuery;
            if(specs.Criteria is not null)
                query = query.Where(specs.Criteria);

            if(specs.Includes is not null)
            query = specs.Includes.Aggregate(query, (current, nextInclude) => current.Include(nextInclude));

            return query;
        }
    }
}
