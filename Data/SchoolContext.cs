using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using NetCoreUniversity.Data;
using NetCoreUniversity.Data.Models;

namespace NetCoreUniversity.Data {
    public class SchoolContext : DbContext
    {
        // To support DI
        public SchoolContext(DbContextOptions options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassStudent> ClassStudents { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            // Define primary keys
            builder.Entity<ClassStudent>()
                 .HasKey(
                    o => new { ClassID = o.ClassId, StudentID = o.StudentId }
                    );

            // Indexes (do not use alternate keys for uniqueness, 
            // EF Core won't allow updating alternate keys)
            builder.Entity<Student>()
                .HasIndex(p => p.Surname);
            builder.Entity<Teacher>()
                .HasIndex(p => p.Surname);

            // Define one-to-many relationships
            // All defined implicitly at the moment

            // Define many-to-many relationship
            // All defined implicitly at the moment

            // Custom column names
            builder.Entity<Class>()
                .Property(c => c.TeacherId)
                .HasColumnName("TeacherPersonId");
            builder.Entity<ClassStudent>()
                .Property(o => o.StudentId)
                .HasColumnName("StudentPersonId");
        }
    }

    // Only to be used by migrations -- not a clean method, I admit
    // add migration won't take a connection string
    internal class SchoolContextFactory : IDbContextFactory<SchoolContext>
    {
        public SchoolContext Create(DbContextFactoryOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;" +
                                        "Initial Catalog=quantumit_school;" +
                                        "Integrated Security=True;" +
                                        "Pooling=False");
            return new SchoolContext(optionsBuilder.Options);
        }
    }
}