using Kafo.DAL.Data;
using KAFO.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace Kafo.DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDBContext _context;
        private DbSet<T> _dbSet;
        public Repository(AppDBContext dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void AddORUpdate(int id, T entity)
        {
            var existing = _dbSet.Find(id);
            if (existing == null)
            {
                Add(entity);
                return;
            }

            _context.Entry(existing).CurrentValues.SetValues(entity);
            _context.SaveChanges();
        }

        public void AddRange(IEnumerable<T> entitie)
        {
            _dbSet.AddRange(entitie);
        }

        public T? FindById(object id)
        {
            return _dbSet.Find(id);
        }

        public T? Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;

            if (tracked)
            {
                query = _dbSet;
            }
            else
            {
                query = _dbSet.AsNoTracking();
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(T)))
            {
                query = query.Where(e => !( (ISoftDelete) e ).IsDeleted);
            }

            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? includeProperties = null, Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(T)))
            {
                query = query.Where(e => !( (ISoftDelete) e ).IsDeleted);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return query.ToList();
        }

        public void Remove(T entity)
        {
            // Soft delete implementation
            if (entity is ISoftDelete softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Remove(entity);
            }
        }

        public void Update(T entity)
        {
            var res = _dbSet.Update(entity);
        }


    }
}
