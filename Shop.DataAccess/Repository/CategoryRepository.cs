using Shop.DataAccess.Data;
using Shop.DataAccess.Repository.IRepository;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}
