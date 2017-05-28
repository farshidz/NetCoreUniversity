using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreUniversity.Data;
using NetCoreUniversity.Data.Models;

namespace NetCoreUniversity.Data {
    public class ClassRepository : GenericRepository<Class>, IClassRepository {
        public ClassRepository(SchoolContext context) : base(context) { }

        public override ICollection<Class> GetAll() {
            return this.dbSet
                .Include(c => c.Teacher)
                .ToList();
        }

        public override Class GetById(object id) {
            // Make sure to maintain original GetByIds behaviour
            if (id == null) {
                throw new ArgumentNullException();
            }
            var classId = id as int?;
            if (classId == null) {
                throw new ArgumentException("Key value must be of type int.");
            }

            return this.dbSet
                .Include(c => c.Teacher)
                .SingleOrDefault(c => c.ClassId == classId);
        }

        public void LoadStudents(Class c) {
            this.context.Entry(c)
                .Collection(o => o.ClassStudents)
                .Load();

            // Now load students in ClassStudent records
            foreach (var cs in c.ClassStudents) {
                this.context.Entry(cs)
                   .Reference(o => o.Student)
                   .Load();
            }
        }

        public override void Patch(Class entity) {
            // Note this code also attaches the entity if not attached
            this.context.Entry(entity).Property(o => o.Location).IsModified = true;
            this.context.Entry(entity).Property(o => o.Name).IsModified = true;
        }

        public override bool Exists(object id) {
            if (id.GetType() != typeof(int)) {
                throw new ArgumentException("id must be an integer");
            }
            return this.dbSet.Any(o => o.ClassId == (int)id);
        }

        public async Task LoadStudentsAsync(Class c) {
            await this.context.Entry(c)
                .Collection(o => o.ClassStudents)
                .LoadAsync();

            // Now load students in ClassStudent records
            foreach (var cs in c.ClassStudents) {
                await this.context.Entry(cs)
                    .Reference(o => o.Student)
                    .LoadAsync();
            }
        }

        public override Task<Class> GetByIdAsync(object id) {
            // Make sure to maintain original GetByIds behaviour
            if (id == null) {
                throw new ArgumentNullException();
            }
            var classId = id as int?;
            if (classId == null) {
                throw new ArgumentException("Key value must be of type int.");
            }

            return this.dbSet
                .Include(c => c.Teacher)
                .SingleOrDefaultAsync(c => c.ClassId == classId);
        }

        public override async Task<bool> ExistsAsync(object id) {
            if (id.GetType() != typeof(int)) {
                throw new ArgumentException("id must be an integer");
            }
            return await this.dbSet.AnyAsync(o => o.ClassId == (int)id);
        }
    }
}