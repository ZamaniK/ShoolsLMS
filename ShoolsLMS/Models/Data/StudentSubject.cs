using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public enum EnrollStatuss
    {
        Rejected = 0,
        Pending = 1,
        Accepted = 2
    }
    public class StudentSubject
    {
        [Display(Name = "Status")]
        [DefaultValue(EnrollStatus.Accepted)]
        public EnrollStatuss Status { get; set; }

        [Key]
        [Required]
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        [Key]
        [Required]
        public string StudentId { get; set; }
        public virtual ApplicationUser Student { get; set; }
    }
}