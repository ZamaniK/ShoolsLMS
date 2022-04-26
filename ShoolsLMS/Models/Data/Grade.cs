using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public class Grade
    {
        [Key]
        [Required]
        public int GradeId { get; set; }
        [Required]
        public string GradeName { get; set; }
        //public Grade ParentCategory { get; set; } = null;
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<StudentGrade> Enrollments { get; set; }

    }
}