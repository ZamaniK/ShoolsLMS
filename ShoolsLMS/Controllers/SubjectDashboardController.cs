using ShoolsLMS.Services;
using ShoolsLMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoolsLMS.Controllers
{
    public class SubjectDashboardController : Controller
    {
        // GET: SubjectDashboard
        GradesService gradeService = new GradesService();
        SubjectsService subjectsService = new SubjectsService();

        //// GET: Accomodations
        public ActionResult Index(int gradeID, int? subjectID)
        {
            SubjectDashboardVM model = new SubjectDashboardVM();

            model.Grade = gradeService.GetGradeByID(gradeID);
            model.Subjects = subjectsService.GetAllSubjectsByGrade(gradeID);

            model.SelectedSubjectID = subjectID.HasValue ? subjectID.Value : model.Subjects.First().SubjectId;
            return View(model);
        }

        public ActionResult Details(int id)
        {
            SubjectDetailsViewModel model = new SubjectDetailsViewModel();

            model.Subjects = subjectsService.GetSubjectByID(id);
            return View(model);
        }

    }
}