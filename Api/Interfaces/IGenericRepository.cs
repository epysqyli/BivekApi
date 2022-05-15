using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;

namespace Api.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Patch(T entity, JsonPatchDocument<T> patch);
    }
}