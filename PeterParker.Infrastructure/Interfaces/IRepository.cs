using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterParker.Infrastructure.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Remove(T entity);
        T GetById(int id);
        IEnumerable<T> GetAll();
    }
}
