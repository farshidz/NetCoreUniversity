using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NetCoreUniversity.Data.Models {

    public class Student : Person {
        [Required]
        [Range(0.0, 4.0)] // Assume Monash GPA
        public float Gpa { get; set; }

        // Internal because should not be modified except by repo
        internal virtual ICollection<ClassStudent> ClassStudents { get; set; }

        [NotMapped]
        public IReadOnlyCollection<Class> Classes {
            get {
                if (this.ClassStudents == null) return null;
                var classes = new LinkedList<Class>();
                foreach (var cs in this.ClassStudents) {
                    classes.AddLast(cs.Class);
                }
                return classes;
            }
        }

        public void AddClass(Class cls) {
            this.AddClass(cls, null);
        }

        public void AddClass(int classId) {
            this.AddClass(null, classId);
        }

        /// <summary>
        /// Adds a Class to this Student using either a Class
        /// instance or a ClassId. Exactly one argument must be null.
        /// </summary>
        /// <param name="cls">A Class or null.</param>
        /// <param name="classId">A ClassId or null.</param>
        private void AddClass(Class cls, int? classId) {
            // Verify pre-condition
            if (cls == null && classId == null
                || cls != null && classId != null) {
                throw new ArgumentException("Exactly one argument must be " +
                                            "null.");
            }
            // Make sure ClassStudents are loaded
            if (this.ClassStudents == null) {
                throw new InvalidOperationException("Classes must be " +
                    "loaded to add a new Class.");
            }
            // Make sure not duplicate -- otherwise will cause a nasty 
            // EF Core exception (of the same type, invalid op)
            var id = cls?.ClassId ?? classId; // never null
            var classStudent = this.ClassStudents
                .SingleOrDefault(cs => cs.StudentId == this.PersonId
                                       && cs.ClassId == id);
            if (classStudent != null) {
                throw new InvalidOperationException("Cannot add duplicate " +
                                                    "Class.");
            }

            // Add
            classStudent = new ClassStudent() {
                StudentId = this.PersonId, // this instance is being tracked
                // anyway, since students have been loaded, but just to be safe
                // we use the ID
                Class = cls,
                ClassId = classId ?? 0
            };
            this.ClassStudents.Add(classStudent);
        }

        public bool RemoveClass(int classId) {
            // Behave like ICollection.Remove -- return true iff
            // found and removed
            var cs = this.ClassStudents
                .SingleOrDefault(o => o.Class.ClassId == classId);
            return this.ClassStudents.Remove(cs);
        }
    }
}
