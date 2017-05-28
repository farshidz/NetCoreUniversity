using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCoreUniversity.Data;
using NetCoreUniversity.Data.Models;

namespace NetCoreUniversity.Data {

    /// <summary>
    /// An EN repository for subtypes of Person.
    /// </summary>
    public abstract class PersonRepository<T> : GenericRepository<T>,
        IPersonRepository<T> where T : Person {

        protected PersonRepository(SchoolContext context) : base(context) { }

        public T GetBySurname(string surname) {
            return this.dbSet.SingleOrDefault(p => p.Surname == surname);
        }

        public override void Patch(T entity) {
            //this.dbSet.Attach(entity);

            this.context.Entry(entity).Property(o => o.Dob).IsModified = true;
            this.context.Entry(entity).Property(o => o.GivenName).IsModified = true;
            this.context.Entry(entity).Property(o => o.Surname).IsModified = true;
        }

        public override bool Exists(object id) {
            if (id.GetType() != typeof(int)) {
                throw new ArgumentException("id must be an integer");
            }
            return this.dbSet.Any(o => o.PersonId == (int)id);
        }

        public override async Task<bool> ExistsAsync(object id) {
            if (id.GetType() != typeof(int)) {
                throw new ArgumentException("id must be an integer");
            }
            return await this.dbSet.AnyAsync(o => o.PersonId == (int)id);
        }

        public async Task<T> GetBySurnameAsync(string surname) {
            return await this.dbSet.SingleOrDefaultAsync(p => p.Surname == surname);
        }

        public abstract void LoadClasses(T entity);
        public abstract Task LoadClassesAsync(T entity);
    }
}
