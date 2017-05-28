using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreUniversity.Data.Models {

    public abstract class Person {
        // Although surname is unique, it may change
        [Key] // annotation for clarity
        public int PersonId { get; set; }

        // Made unique using fluent API (SchoolContext)
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Surname { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 40)]
        [RegularExpression("^[a-zA-Z\\s]*$")]
        public string GivenName { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 1)]
        [RegularExpression("^[a-zA-Z\\./]*$")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
    }
}
