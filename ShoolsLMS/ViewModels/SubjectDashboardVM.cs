using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.ViewModels
{
    
    public class SubjectDashboardVM
    {
        public Grade Grade { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
        public int SelectedSubjectID { get; set; }
    }

    public class SubjectDetailsViewModel
    {
        public Subject Subjects { get; set; }
    }
}