using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public class Paper
    {   
        [Key]
        public int PaperId { get; set; }
        public string Title { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd - MM - yyyy}", ApplyFormatInEditMode = true)]

        public DateTime? Date { get; set; }
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

    }
}