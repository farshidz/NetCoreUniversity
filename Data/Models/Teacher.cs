using System.Collections.Generic;

namespace NetCoreUniversity.Data.Models {

    public class Teacher : Person {
        public virtual ICollection<Class> Classes { get; set; }
    }
}
