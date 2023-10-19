using Shop.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        int Complete();
    }
}
