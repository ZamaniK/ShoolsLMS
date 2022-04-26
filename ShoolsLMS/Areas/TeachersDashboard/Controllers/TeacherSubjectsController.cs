using Microsoft.AspNet.Identity;
using ShoolsLMS.Areas.Dashboard.ViewModels;
using ShoolsLMS.Models.Data;
using ShoolsLMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoolsLMS.Areas.TeachersDashboard.Controllers
{
    public class TeacherSubjectsController : Controller
    {
        // GET: Dashboard/Subjects
        SubjectsService subjectsService = new SubjectsService();
        GradesService gradeService = new GradesService();
        DashboardService dashboardService = new DashboardService();
        // GET: Dashboard/Subjects
        public ActionResult Index(string searchTerm, int? gradeID, int? page)
        {
            SubjectListingModel model = new SubjectListingModel();

            int recordSize = 15;
            page = page ?? 1;

            model.SearchTerm = searchTerm;
            model.GradeID = gradeID;

            model.Subjects = subjectsService.SearchSubject(searchTerm, gradeID, page.Value, recordSize);

            model.Grades = gradeService.GetAllGrades();

            var totalRecords = subjectsService.SearchSubjectCount(searchTerm, gradeID);
            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);
        }

        [HttpGet]
        public ActionResult Action(int? ID)
        {
            SubjectActionModel model = new SubjectActionModel();

            if (ID.HasValue)  //we are trying to edit a record
            {
                var subject = subjectsService.GetSubjectByID(ID.Value);
                string getuser = User.Identity.GetUserId();

                model.ID = subject.SubjectId;
                model.GradeId = subject.GradeId;
                model.Name = subject.SubjectName;
                model.Description = subject.Description;
                model.Code = subject.SubjectCode;
                model.OpenDate = subject.StartDate;
                subject.User.Id = getuser;

                model.SubjectPictures = subjectsService.GetPicturesBySubjectID(subject.SubjectId);
            }
            model.Grades = gradeService.GetAllGrades();
            return PartialView("_Action", model);
        }


        [HttpPost]
        public JsonResult Action(SubjectActionModel model)
        {
            JsonResult json = new JsonResult();

            var result = false;
            List<int> pictureIDs = !string.IsNullOrEmpty(model.PictureIDs) ? model.PictureIDs.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>();
            var pictures = dashboardService.GetPicturesByID(pictureIDs);

            if (model.ID > 0)
            {
                var subject = subjectsService.GetSubjectByID(model.ID);
                string getuser = User.Identity.GetUserId();


                subject.SubjectId = model.ID;
                subject.GradeId = model.GradeId;
                subject.SubjectName = model.Name;
                subject.Description = model.Description;
                subject.SubjectCode = model.Code;
                subject.StartDate = DateTime.Now;
                subject.ApplicationUserID = getuser;
                subject.SubjectPictures.Clear();
                subject.SubjectPictures.AddRange(pictures.Select(x => new SubjectPictures() { SubjectId = subject.SubjectId, PictureID = x.ID }));

                result = subjectsService.UpdateSubject(subject);
            }
            else
            {
                Subject subject = new Subject();

                string getuser = User.Identity.GetUserId();


                model.ID = subject.SubjectId;
                subject.GradeId = model.GradeId;
                subject.SubjectName = model.Name;
                subject.Description = model.Description;
                subject.SubjectCode = model.Code;
                subject.StartDate = DateTime.Now;
                subject.ApplicationUserID = getuser;




                subject.SubjectPictures = new List<SubjectPictures>();
                subject.SubjectPictures.AddRange(pictures.Select(x => new SubjectPictures() { PictureID = x.ID }));

                result = subjectsService.SaveSubject(subject);
            }


            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on Subjects." };
            }
            return json;
        }


        [HttpGet]
        public ActionResult Delete(int ID)
        {
            SubjectActionModel model = new SubjectActionModel();

            var subject = subjectsService.GetSubjectByID(ID);

            model.ID = subject.SubjectId;


            return PartialView("_Delete", model);
        }


        [HttpPost]
        public JsonResult Delete(SubjectActionModel model)
        {
            JsonResult json = new JsonResult();

            var result = false;

            var subject = subjectsService.GetSubjectByID(model.ID);



            result = subjectsService.DeleteSubject(subject);

            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on Subjects." };
            }
            return json;
        }
    }
}