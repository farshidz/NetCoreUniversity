using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataAccess.Dal;

namespace NetCoreUniversity.Data {
    // TODO: Add documentation to clarify return value in special cases (e.g., 
    // does not exist) and possible exceptions.

    // Based on http://farsh.id/ENPwM (docs.microsoft)
    // Modified by Farshid Zavareh, including async methods and other changes
    // as the article is old

    /// <summary>
    /// Generic repository class. This class has no abstract methods,
    /// but for clarity, it should only be used through concrete subclasses.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GenericRepository<T> : IGenericRepository<T>
        where T : class {

        protected SchoolContext context;
        protected DbSet<T> dbSet;

        protected GenericRepository(SchoolContext context) {
            if (context == null)
                throw new ArgumentNullException($"{nameof(context)} " +
                                                 $"cannot be null");
            this.context = context;
            this.dbSet = this.context.Set<T>();
        }

        // This is (much) more efficient than retrieving all records,
        // the filtering
        public virtual ICollection<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "") {

            IQueryable<T> query = this.dbSet;

            if (filter != null) {
                query = query.Where(filter);
            }

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                query = query.Include(includeProperty);
            }

            if (orderBy != null) {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public virtual ICollection<T> GetAll() {
            return this.dbSet.ToList();
        }

        public virtual T GetById(object id) {
            return this.dbSet.Find(id);
        }

        public virtual void Insert(T entity) {
            this.dbSet.Add(entity);
        }

        public virtual void Delete(object id) {
            var entityToDelete = this.GetById(id);

            this.dbSet.Remove(entityToDelete);
        }

        public virtual void Delete(T entityToDelete) {
            this.dbSet.Remove(entityToDelete);
        }

        /// <summary>
        /// Marks an entity to be updated when 
        /// <see cref="IDataRepositories.Save()"/> or 
        /// <see cref="IDataRepositories.SaveAsync()"/>
        /// is called.
        /// </summary>
        /// <param name="entityToUpdate">The entity to mark for update.</param>
        public virtual void Update(T entityToUpdate)
        {
            this.dbSet.Update(entityToUpdate);
            //this.context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public abstract void Patch(T entity);
        public abstract bool Exists(object id);

        /* async versions of everything */

        public virtual async Task<ICollection<T>> GetAsync(
             Expression<Func<T, bool>> filter = null,
             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
             string includeProperties = "") {

            IQueryable<T> query = this.dbSet;

            if (filter != null) {
                query = query.Where(filter);
            }

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                query = query.Include(includeProperty);
            }

            if (orderBy != null) {
                return await orderBy(query).ToListAsync();
            } // else
            return await query.ToListAsync();
        }

        public virtual async Task<ICollection<T>> GetAllAsync() {
            return await this.dbSet.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(object id) {
            return await this.dbSet.FindAsync(id);
        }

        public virtual async Task DeleteAsync(object id) {
            var entityToDelete = await this.GetByIdAsync(id);

            this.dbSet.Remove(entityToDelete);
        }

        public abstract Task<bool> ExistsAsync(object id);
    }
}
