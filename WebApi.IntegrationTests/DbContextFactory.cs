using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetCoreUniversity.Data;
using NetCoreUniversity.Data.Models;

namespace NetCoreUniversity.WebApi.IntegrationTests {
    public class DbContextFactory {
        // This keeps track of which db we are using now
        // To reset the db (=get fresh db), set to null
        private static string dbName;

        public SchoolContext GetContext() {
            bool populate = false;
            if (dbName == null) {
                dbName = Guid.NewGuid().ToString();
                populate = true;
            }

            var options = new DbContextOptionsBuilder<SchoolContext>()
              .UseInMemoryDatabase(databaseName: dbName)
              .Options;

            if (populate) {
                this.Populate(new SchoolContext(options));
            }

            return new SchoolContext(options);
        }

        public void Reset() {
            dbName = null;
        }

        private void Populate(SchoolContext context) {
            var teachers = this.CreateTestTeachers();
            var students = this.CreateTestStudents();
            var classes = this.CreateTestClasses(students, teachers);

            context.Teachers.AddRange(teachers);
            context.Students.AddRange(students);
            context.Classes.AddRange(classes);

            context.SaveChanges();
        }


        #region Test Data Generation

        private ICollection<Class> CreateTestClasses(ICollection<Student> students,
            ICollection<Teacher> teachers) {

            var teacher = teachers.First();

            var classes = new List<Class>
            {
                new Class()
                {
                    Location = "1-131 Wellington Road Clayton",
                    Name = "Class One",
                    Teacher = teacher
                },
                new Class()
                {
                    Location = "1-131 Wellington Road Clayton",
                    Name = "Class Two",
                    Teacher = teacher
                }
            };
            // Assign students
            foreach (var c in classes) {
                c.Students = students;
            }
            // Assign classes to teacher
            teacher.Classes = new List<Class>()
            {
                classes[0],
                classes[1]
            };

            return classes;
        }

        private ICollection<Student> CreateTestStudents() {
            var students = new List<Student>
            {
                new Student()
                {
                    Dob = DateTime.Now.AddYears(-18),
                    GivenName = "John",
                    Surname = "StudentOne",
                    Title = "Mr",
                    Gpa = 3.5F,
                    PersonId = 1
                },
                new Student()
                {
                    Dob = DateTime.Now.AddYears(-20),
                    GivenName = "Marry",
                    Surname = "StudentTwo",
                    Title = "Ms",
                    Gpa = 2.1F,
                    PersonId = 2
                },
                new Student()
                {
                    Dob = DateTime.Now.AddYears(-20),
                    GivenName = "Marry",
                    Surname = "StudentThree",
                    Title = "Ms",
                    Gpa = 2.1F,
                    PersonId = 3
                }
            };

            return students;
        }

        private ICollection<Teacher> CreateTestTeachers() {
            var teacher = new Teacher() {
                PersonId = 4,
                Dob = DateTime.Now.AddYears(-25),
                GivenName = "John",
                Surname = "Smith",
                Title = "Mr"
            };

            return new List<Teacher>()
            {
                teacher
            };
        }

        #endregion
    }
}
