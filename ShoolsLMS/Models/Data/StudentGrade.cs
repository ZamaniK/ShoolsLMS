using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public enum EnrollStatus
    {
        Rejected = 0,
        Pending = 1,
        Accepted = 2
    }
    public class StudentGrade
    {
        [Display(Name = "Status")]
        [DefaultValue(EnrollStatus.Accepted)]
        public EnrollStatus Status { get; set; }

        [Key]
        [Required]
        public int GradeId { get; set; }
        public virtual Grade Grade { get; set; }

        [Key]
        [Required]
        public string StudentId { get; set; }
        public virtual ApplicationUser Student { get; set; }
    }
}