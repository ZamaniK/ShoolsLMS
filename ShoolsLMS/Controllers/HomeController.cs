using ShoolsLMS.Services;
using ShoolsLMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoolsLMS.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            SubjectsVM model = new SubjectsVM();
            GradesService gradeService = new GradesService();
            SubjectsService subjectService = new SubjectsService();

            model.Grades = gradeService.GetAllGrades();
            model.Subjects = subjectService.GetAllSubjects();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}