using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NetCoreUniversity.Data;
using NetCoreUniversity.Data.Models;

namespace NetCoreUniversity.Data {
    public class StudentRepository : PersonRepository<Student> {
        public StudentRepository(SchoolContext context) : base(context) { }

        public override void LoadClasses(Student student) {
            this.context.Entry(student)
                .Collection(o => o.ClassStudents)
                .Load();

            // Now load classes in ClassStudent records
            foreach (var cs in student.ClassStudents) {
                this.context.Entry(cs)
                    .Reference(o => o.Class)
                    .Load();
            }
        }

        public override void Patch(Student entity) {
            base.Patch(entity);
            this.context.Entry(entity).Property(o => o.Gpa).IsModified = true;
        }

        public override async Task LoadClassesAsync(Student student) {
            await this.context.Entry(student)
                 .Collection(o => o.ClassStudents)
                 .LoadAsync();

            // Now load classes in ClassStudent records
            foreach (var cs in student.ClassStudents) {
                await this.context.Entry(cs)
                    .Reference(o => o.Class)
                    .LoadAsync();
            }
        }
    }
}
