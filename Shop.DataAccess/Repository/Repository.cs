using Microsoft.EntityFrameworkCore;
using Shop.DataAccess.Data;
using Shop.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(T obj)
        {
            _context.Set<T>().Add(obj);
        }

        public void Delete(T obj)
        {
            _context.Set<T>().Remove(obj);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public T Get(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public void Update(T obj)
        {
            _context.Set<T>().Update(obj);
        }
    }
}
