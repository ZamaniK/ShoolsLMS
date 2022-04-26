using ShoolsLMS.Services;
using ShoolsLMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoolsLMS.Controllers
{
    public class SubjectGradeController : Controller
    {
        // GET: Subjects
        public ActionResult Index()
        {
            SubjectsVM model = new SubjectsVM();
            GradesService gradeService = new GradesService();
            SubjectsService subjectService = new SubjectsService();

            model.Grades = gradeService.GetAllGrades();
            model.Subjects = subjectService.GetAllSubjects();
            return View(model);
        }
    }
}