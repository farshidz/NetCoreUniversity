using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NetCoreUniversity.Data.Models;

namespace NetCoreUniversity.Data {
    // Important note: Since EF Core does not support lazy loading, 
    // we are providing methods for explicit loading, in order to avoid
    // eager loading unnecessarily.

    public interface IGenericRepository<T> where T : class {
        ICollection<T> GetAll();
        ICollection<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
        T GetById(object id);
        void Insert(T entity);
        void Delete(T entity);
        void Delete(object id);
        void Update(T entity);
        /// <summary>
        /// Partially updates an entity, updating only its non-navigational
        /// properties.
        /// </summary>
        /// <param name="entity"></param>
        void Patch(T entity);
        bool Exists(object id);

        // async versions
        Task<ICollection<T>> GetAllAsync();
        Task<ICollection<T>> GetAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
        Task<T> GetByIdAsync(object id);
        Task<bool> ExistsAsync(object entity);
    }

    /// <summary>
    /// Defines methods to manupulate Person records
    /// in the underlying datasource. 
    /// </summary>
    public interface IPersonRepository<T> : IGenericRepository<T> where T : Person {
        T GetBySurname(string surname);
        void LoadClasses(T entity);

        Task<T> GetBySurnameAsync(string username);
        Task LoadClassesAsync(T entity);
    }

    public interface IClassRepository : IGenericRepository<Class> {
        void LoadStudents(Class c);

        Task LoadStudentsAsync(Class c);
    }

    /// <summary>
    /// An implementation of Unit of Work pattern to support manipulation
    /// of the underlying datasource.
    /// </summary>
    public interface IDataRepositories {
        IPersonRepository<Student> StudentRepository();
        IPersonRepository<Teacher> TeacherRepository();
        IClassRepository ClassRepository();
        /// <summary>
        /// Flushes all changes to the datasource.
        /// </summary>
        void Save();

        Task SaveAsync();
    }
}
