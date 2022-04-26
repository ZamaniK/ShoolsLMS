using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Areas.Dashboard.ViewModels
{
    public class AssignmentListingModel
    {
        public IEnumerable<Assignment> Assignments { get; set; }
        public string SearchTerm { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
        public int? SubjectID { get; set; }
        public Pager Pager { get; set; }

    }
}