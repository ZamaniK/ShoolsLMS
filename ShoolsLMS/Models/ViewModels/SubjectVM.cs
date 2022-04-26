using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.ViewModels
{
    public class SubjectVM
    {
        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        [Required]
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }

        [Required(ErrorMessage = "The subject must have a description!")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Grade")]
        public int GradeId { get; set; }
        public Grade Grade { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [ScaffoldColumn(false)]
        public int Rating { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<StudentGrade> Enrollments { get; set; }
        public virtual ICollection<Lesson> Teachers { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}