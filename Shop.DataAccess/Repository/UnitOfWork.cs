using Shop.DataAccess.Data;
using Shop.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository Categories { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Categories = new CategoryRepository(context);
        }


        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
