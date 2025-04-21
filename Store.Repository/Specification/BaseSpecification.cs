using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        #region Criteria
        public Expression<Func<T, bool>> Criteria { get; }
        #endregion

        #region Includes
        public List<Expression<Func<T, object>>> Includes { get; } = 
            new List<Expression<Func<T, object>>>();

        protected void AddIncludes(Expression<Func<T, object>> include)
           => Includes.Add(include);
        #endregion

        #region Ordering
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
            => OrderBy = orderByExpression;
        protected void AddOrderByDesending(Expression<Func<T, object>> orderByDescExpression)
            => OrderBy = orderByDescExpression;
        #endregion

        #region Pagination
        public int Skip { get; private set; }
        public int Take { get; private set; }
        public bool IsPaginated { get; private set; }

        protected void ApplyPagination(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPaginated = true;
        }
        #endregion

    }
}
