using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HelpdeskDAL
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAll();
        Task<List<T>> GetSome(Expression<Func<T, bool>> match);
        Task<T?> GetOne(Expression<Func<T, bool>> match);
        // The missing method that retrieves an entity by its ID

        Task<T> Add(T entity);
        Task<UpdateStatus> Update(T enity);
        Task<int> Delete(int i);
        Task<Employee?> GetOne(int id);
    }
}
