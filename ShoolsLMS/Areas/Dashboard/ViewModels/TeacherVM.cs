using ShoolsLMS.Models;
using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Areas.Dashboard.ViewModels
{
    public class TeacherListingModel
    {
        public IEnumerable<Teacher> Teachers { get; set; }
        public string SearchTerm { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
        public int? SubjectID { get; set; }
        public Pager Pager { get; set; }

    }

    public class TeacherActionModel
    {
        public int ID { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public string Name { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string PictureIDs { get; set; }

        public IEnumerable<Subject> Subjects { get; set; }
        public List<TeacherPictures> TeacherPictures { get; set; }

    }
}