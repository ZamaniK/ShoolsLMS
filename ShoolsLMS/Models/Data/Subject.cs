using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        [Required]
        [MaxLength(20)]
        //[RegularExpression(@"^\S\,*$", ErrorMessage = "No white space allowed")]
        [Display(Name = "Subject Code")]
        public string SubjectCode { get; set; }

        [Required]
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }

        [Required(ErrorMessage = "The subject must have a description!")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        public string ApplicationUserID { get; set; }

        [Required]
        //[ForeignKey("Grade")]
        [Display(Name = "Grade")]
        public int GradeId { get; set; }
        public virtual Grade Grade { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd - MM - yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }


        [ScaffoldColumn(false)]
        public int Rating { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<Paper> Papers { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Assignment> Assignment{ get; set; }


        public virtual ICollection<StudentAssignment> Assignments { get; set; }
        public virtual ICollection<StudentSubject> Enrollments { get; set; }

        public virtual List<SubjectPictures> SubjectPictures { get; set; }
    }


}