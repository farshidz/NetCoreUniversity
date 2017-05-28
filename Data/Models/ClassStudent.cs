namespace NetCoreUniversity.Data.Models {
    /// <summary>
    /// This class is a join table for the many-to-many relationship between Class and Student, which
    /// is not currently supported by Entity Framework Core otherwise. 
    /// </summary>
    public class ClassStudent {
        // Note: We are assuming that classes will always have
        // one teacher -- otherwise generic Person would have 
        // been apter
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int ClassId { get; set; }
        public Class Class { get; set; }
    }
}
