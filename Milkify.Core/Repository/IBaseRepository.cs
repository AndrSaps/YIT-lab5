using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Milkify.Core.Repository
{
    public interface IBaseRepository : IDisposable
    {
        DbSet<TEntity> DbSet<TEntity>() where TEntity : class;
        
        /// <summary>
        /// Insert new record to DB
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        TEntity Insert<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        void Remove<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Create query for the specified entity.
        /// </summary>
        IQueryable<TEntity> Query<TEntity>() where TEntity : class;

        /// <summary>
        /// Create query for the specified entity.
        /// </summary>
        IQueryable<TEntity> Query<TEntity>(Expression<Func<TEntity, bool>> selector) where TEntity : class;

        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="id">The entity id</param>
        TEntity Get<TEntity>(long id) where TEntity : class;

        /// <summary>
        /// Get entity by custom query.
        /// </summary>
        /// <param name="selector">The custom query</param>
        TEntity Get<TEntity>(Expression<Func<TEntity, bool>> selector) where TEntity : class;

        /// <summary>
        /// Update existing record in DB
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        void Update<TEntity>(TEntity entity) where TEntity : class;
        
        bool Any<T>(Expression<Func<T, bool>> expression) where T : class;
        bool All<T>(Expression<Func<T, bool>> expression) where T : class;
        /// <summary>
        /// Saves the changes.
        /// </summary>
        void SaveChanges();

        DbContext Context { get; }
        
    }
}
