using ShoolsLMS.Areas.Dashboard.ViewModels;
using ShoolsLMS.Models.Data;
using ShoolsLMS.Services;
using System.Web.Mvc;

namespace ShoolsLMS.Areas.Dashboard.Controllers
{
    public class GradesController : Controller
    {
        GradesService gradesService = new GradesService();
        // GET: Dashboard/Grades
        public ActionResult Index1(string searchTerm, int? page)
        {
            GradeListingModel model = new GradeListingModel();

            int recordSize = 5;
            page = page ?? 1;

            model.SearchTerm = searchTerm;

            model.Grades = gradesService.SearchGrades(searchTerm, page.Value, recordSize);

            var totalRecords = gradesService.SearchGradeCount(searchTerm);
            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);
        }

        [HttpGet]
        public ActionResult Action(int? ID)
        {
            GradeActionModel model = new GradeActionModel();

            if (ID.HasValue)  //we are trying to edit a record
            {
                var grade = gradesService.GetGradeByID(ID.Value);

                model.ID = grade.GradeId;
                model.Name = grade.GradeName;
            }
            return PartialView("_Action", model);
        }


        [HttpPost]
        public JsonResult Action(GradeActionModel model)
        {
            JsonResult json = new JsonResult();

            var result = false;

            if (model.ID > 0)
            {
                var grade = gradesService.GetGradeByID(model.ID);

                grade.GradeName = model.Name;

                result = gradesService.UpdateGrade(grade);
            }
            else
            {
                Grade grade = new Grade
                {
                    GradeName = model.Name
                };

                result = gradesService.SaveGrade(grade);
            }


            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on Grade." };
            }
            return json;
        }


        [HttpGet]
        public ActionResult Delete(int ID)
        {
            GradeActionModel model = new GradeActionModel();

            var grade = gradesService.GetGradeByID(ID);

            model.ID = grade.GradeId;


            return PartialView("_Delete", model);
        }


        [HttpPost]
        public JsonResult Delete(GradeActionModel model)
        {
            JsonResult json = new JsonResult();

            var result = false;

            var grade = gradesService.GetGradeByID(model.ID);

            result = gradesService.DeleteGrade(grade);

            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on Grade." };
            }
            return json;
        }


    }
}