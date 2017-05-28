using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NetCoreUniversity.Data.Models {

    public class Class {
        [Key] // annotation for clarity
        public int ClassId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 3)]
        public string Location { get; set; }

        public int? TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        // Virtual to enable lazy loading (edit: but EF Core doesn't support 
        // lazy loading)
        // Internal because we don't want it accessed or modified directly
        /// <summary>
        /// Class Student join table. Both set and get methods are intended
        /// only for the ORM.
        /// </summary>
        internal virtual ICollection<ClassStudent> ClassStudents {
            // ORM only
            get {
                if (Students == null) return null;
                var classStudents = new LinkedList<ClassStudent>();

                foreach (var student in Students) {
                    var classStudent = classStudents
                        .SingleOrDefault(cs => cs.StudentId == student.PersonId
                        && cs.ClassId == this.ClassId);
                    if (classStudent != null) {
                        throw new InvalidOperationException("Cannot add " +
                            "duplicate Student.");
                    }
                    // Add
                    classStudent = new ClassStudent() {
                        ClassId = this.ClassId, // this instance is being tracked
                                                // anyway, since students have been loaded, but just to be safe
                                                // we use the ID
                        Student = student
                    };
                    this.ClassStudents.Add(classStudent);
                }

                return classStudents;
            }
            // ORM only
            set {
                var classStudents = value;
                // Now create Students
                if (classStudents == null) return;
                var students = new LinkedList<Student>();
                foreach (var cs in classStudents) {
                    // Students must be eager-loaded for this to work
                    if (cs.Student != null) students.AddLast(cs.Student);
                }
                this.Students = students;
            }
        }

        [NotMapped]
        public ICollection<Student> Students { get; set; }

        public bool RemoveStudent(int personId) {
            // Behave like ICollection.Remove -- return true iff
            // found and removed
            var cs = this.ClassStudents
                .SingleOrDefault(o => o.Student.PersonId == personId);
            return this.ClassStudents.Remove(cs);
        }
    }
}
