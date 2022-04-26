using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.ViewModels
{
    public class SubjectsVM
    {
        public IEnumerable<Grade> Grades { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
    }
}