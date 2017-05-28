using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NetCoreUniversity.Data;
using NetCoreUniversity.Data.Models;

namespace NetCoreUniversity.Data {
    public class TeacherRepository : PersonRepository<Teacher> {
        public TeacherRepository(SchoolContext context) : base(context) { }

        public override void LoadClasses(Teacher entity) {
            this.context.Entry(entity)
                .Collection(o => o.Classes)
                .Load();
        }

        public override async Task LoadClassesAsync(Teacher entity) {
            await this.context.Entry(entity)
                .Collection(o => o.Classes)
                .LoadAsync();
        }
    }
}
