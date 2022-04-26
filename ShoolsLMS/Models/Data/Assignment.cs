using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public class Assignment
    {
        [Key]
        public int AssignmentId { get; set; }
        [Required]
        public string AssignmentName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd - MM - yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastDate { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }

        [Required]
        public int SubjectId { get; set; }
        public virtual Subject Subject   { get; set; }
        public ICollection<StudentAssignment> Assignments { get; set; }
    }
}