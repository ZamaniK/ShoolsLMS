using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Areas.Dashboard.ViewModels
{
    public class GradeListingModel
    {
        public IEnumerable<Grade> Grades { get; set; }
        public string SearchTerm { get; set; }
        public Pager Pager { get; set; }

    }

    public class GradeActionModel
    {
        public int ID { get; set; }

        public string Name { get; set; } 

    }
}