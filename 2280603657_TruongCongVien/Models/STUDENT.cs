namespace _2280603657_TruongCongVien.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("STUDENTS")]
    public partial class STUDENT
    {
        [StringLength(20)]
        public string StudentID { get; set; }

        [Required]
        [StringLength(200)]
        public string StudentName { get; set; }

        public double AverageScore { get; set; }

        public int FacultyID { get; set; }

        public virtual FACULTY FACULTY { get; set; }
    }
}
