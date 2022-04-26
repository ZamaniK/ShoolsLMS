using ShoolsLMS.Areas.Dashboard.ViewModels;
using ShoolsLMS.Models;
using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Areas.TeachersDashboard.ViewModels
{
    public class SubjectListingModel
    {
        public IEnumerable<Subject> Subjects { get; set; }
        public string SearchTerm { get; set; }
        public IEnumerable<Grade> Grades { get; set; }
        public int? GradeID { get; set; }
        public Pager Pager { get; set; }

    }

    public class SubjectActionModel
    {
        public int ID { get; set; }

        public int GradeId { get; set; }
        public Grade Grade { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public virtual ApplicationUser User { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd - MM - yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? OpenDate { get; set; }
        public string PictureIDs { get; set; }

        public IEnumerable<Grade> Grades { get; set; }
        public List<SubjectPictures> SubjectPictures { get; set; }


    }
}