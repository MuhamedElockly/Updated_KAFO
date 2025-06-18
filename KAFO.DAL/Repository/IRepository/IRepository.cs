using System.Linq.Expressions;


namespace Kafo.DAL.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string? includeProperties = null, Expression<Func<T, bool>>? filter = null);
        T? Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true);
        T? FindById(object id);

        //tracked, not new
        void Update(T entity);
        void AddORUpdate(int id, T entity);
        void Add(T entity);
        void AddRange(IEnumerable<T> entitie);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

    }
}
