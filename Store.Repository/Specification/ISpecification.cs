using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification
{
    public interface ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; } //.Where(...), x => x.id == id
        public List<Expression<Func<T, object>>> Includes { get; }
        public Expression<Func<T, object>> OrderBy { get; }
        public Expression<Func<T, object>> OrderByDescending { get; }


    }
}
