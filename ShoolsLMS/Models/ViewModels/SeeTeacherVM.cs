using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.ViewModels
{
    public class SeeTeacherVM
    {
        [Key]
        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        [Display(Name = "Subject Code")]
        public string SubjectCode { get; set; }

        [Required]
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Teacher UserName")]
        public string TeacherUserName { get; set; }
        public ICollection<Lesson> Teachers { get; set; }
    }
}