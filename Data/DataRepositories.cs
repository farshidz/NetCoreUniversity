using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using NetCoreUniversity.Data;
using NetCoreUniversity.Data.Models;

namespace NetCoreUniversity.Data {

    /// <summary>
    /// An implementation of Unit of Work pattern to support manupulating the un
    /// </summary>
    public class DataRepositories : IDataRepositories {

        private readonly SchoolContext context;
        private IPersonRepository<Student> studentRepository;
        private IPersonRepository<Teacher> teacherRepository;
        private IClassRepository classRepository;

        public DataRepositories(SchoolContext context) {
            this.context = context;
        }

        public IPersonRepository<Student> StudentRepository() {
            if (this.studentRepository == null) {
                this.studentRepository = new StudentRepository(this.context);
            }
            return this.studentRepository;
        }

        public IPersonRepository<Teacher> TeacherRepository() {
            if (this.teacherRepository == null) {
                this.teacherRepository = new TeacherRepository(this.context);
            }
            return this.teacherRepository;
        }

        public IClassRepository ClassRepository() {
            if (this.classRepository == null) {
                this.classRepository = new ClassRepository(this.context);
            }
            return this.classRepository;
        }

        /// <summary>
        /// Saves changed to the databse.
        /// </summary>
        /// <exception cref="NetCoreUniversity.Data.DbUpdateConcurrencyException">
        /// Thrown often when an entity that is being modified (or one of its
        /// navigational properties) does not correspond to a record in the 
        /// database with the same primary key.
        /// </exception>
        /// <exception cref="NetCoreUniversity.Data.DbUpdateException">
        /// Thrown when updating the database fails for any reason, including
        /// constraint violations.
        /// </exception>
        public void Save() {
            try {
                this.context.SaveChanges();
            } catch (DbUpdateConcurrencyException e) {
                throw new global::DataAccess.Dal.DbUpdateConcurrencyException(
                    innerException: e);
            } catch (DbUpdateException e) {
                throw new global::DataAccess.Dal.DbUpdateException(
                    innerException: e);
            }
        }

        /// <summary>
        /// Saves changed to the databse.
        /// </summary>
        /// <exception cref="DataAccess.Dal.DbUpdateConcurrencyException">
        /// Thrown often when an entity that is being modified (or one of its
        /// navigational properties) does not correspond to a record in the 
        /// database with the same primary key.
        /// </exception>
        /// <exception cref="DataAccess.Dal.DbUpdateException">
        /// Thrown when updating the database fails for any reason, including
        /// constraint violations.
        /// </exception>
        public async Task SaveAsync() {
            try {
                this.context.SaveChanges();
                await this.context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException e) {
                throw new DataAccess.Dal.DbUpdateConcurrencyException(
                    innerException: e);
            } catch (DbUpdateException e) {
                throw new DataAccess.Dal.DbUpdateException(
                    innerException: e);
            }
        }
    }
}
