using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.ViewModels
{
    public class GradeViewModel
    {
        [Display(Name = "Grade")]
        public int GradeId { get; set; }

        [Required]
        [Display(Name = "Grade Name")]
        public string GradeName { get; set; }
    }
}