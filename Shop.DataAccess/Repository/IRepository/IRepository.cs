using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        void Add(T obj);
        void Delete(T obj);
        IEnumerable<T> GetAll(string? includeProperties = null);
        T Find(Expression<Func<T, bool>> predicate, string? includeProperties = null);
        void Update(T obj);
    }
}
