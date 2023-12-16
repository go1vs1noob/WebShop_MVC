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

        public T Find(Expression<Func<T, bool>> predicate, string? includeProperties = null)
        {
            IQueryable<T> dbSet = _context.Set<T>();
            foreach (var property in parseIncludeProperties(includeProperties))
            {
                dbSet = _context.Set<T>().Include<T>(property);
            }
            return dbSet.Where(predicate).FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> dbSet = _context.Set<T>();
            foreach (var property in parseIncludeProperties(includeProperties))
            {
                dbSet = _context.Set<T>().Include<T>(property);
            }
            return dbSet.ToList();
        }
        public void Update(T obj)
        {
            _context.Set<T>().Update(obj);
        }
        private IEnumerable<string> parseIncludeProperties(string? includeProperties)
        {
            if (includeProperties == null)
            {
                return Enumerable.Empty<string>();
            }
            return includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }

}
