using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Milkify.Core;
using Milkify.Core.Repository;
using Milkify.DAL.Data;

namespace Milkify.DAL.Repository
{
    public class BaseRepository : IBaseRepository
    {
        private readonly ApplicationDbContext _context;

        public DbContext Context => _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public DbSet<TEntity> DbSet<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class
        {
            return Context.Set<TEntity>().Add(entity).Entity;
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            return Context.Set<TEntity>();
        }

        public IQueryable<TEntity> Query<TEntity>(Expression<Func<TEntity, bool>> selector) where TEntity : class
        {
            return Context.Set<TEntity>().Where(selector);
        }

        public TEntity Get<TEntity>(long id) where TEntity : class
        {
            return Context.Set<TEntity>().Find(id);
        }

        public TEntity Get<TEntity>(Expression<Func<TEntity, bool>> selector) where TEntity : class
        {
            return Context.Set<TEntity>().FirstOrDefault(selector);
        }

        public bool All<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return Context.Set<T>().All(expression);
        }
        
        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            Context.Set<TEntity>().Update(entity);
        }

        public bool Any<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return Context.Set<T>().Any(expression);
        }

        public void Dispose()
        {
            Context.Dispose();
        }
        

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

    }
}
