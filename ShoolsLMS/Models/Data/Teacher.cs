using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public class Teacher
    {
        [Key]
        [Display(Name = "Teacher's No.")]
        public int TeacherId { get; set; }

        [Required]
        [Display(Name ="Teacher's Name")]
        public string TeacherName { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }
        public string ApplicationUserID { get; set; }
        public ApplicationUser User { get; set; }

        public virtual List<TeacherPictures> TeacherPictures { get; set; }

    }
}