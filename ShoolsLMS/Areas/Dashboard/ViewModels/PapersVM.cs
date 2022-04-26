using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Areas.Dashboard.ViewModels
{
    public class PapersListingModel
    {
        public IEnumerable<Paper> Papers { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }

        public string SearchTerm { get; set; }
        public int? SubjectID { get; set; }
        public Pager Pager { get; set; }

    }

    public class PaperActionModel
    {
        public int ID { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public string Title { get; set; }
        public string FileName { get; set; }
        public DateTime? Date { get; set; }

        public IEnumerable<Subject> Subjects { get; set; }

    }
}