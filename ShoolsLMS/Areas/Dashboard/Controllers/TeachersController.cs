using Microsoft.AspNet.Identity;
using ShoolsLMS.Areas.Dashboard.ViewModels;
using ShoolsLMS.Models.Data;
using ShoolsLMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoolsLMS.Areas.Dashboard.Controllers
{
    public class TeachersController : Controller
    {
        // GET: Dashboard/Teachers
        TeachersService teacherService = new TeachersService();
        SubjectsService subjectService = new SubjectsService();
        DashboardService dashboardService = new DashboardService();
        public ActionResult Index(string searchTerm, int? subjectID, int? page)
        {
            TeacherListingModel model = new TeacherListingModel();

            int recordSize = 5;
            page = page ?? 1;

            model.SearchTerm = searchTerm;
            model.SubjectID = subjectID;

            model.Teachers = teacherService.SearchTeacher(searchTerm, subjectID, page.Value, recordSize);

            model.Subjects = subjectService.GetAllSubjects();

            var totalRecords = teacherService.SearchTeacherCount(searchTerm, subjectID);
            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);
        }

        [HttpGet]
        public ActionResult Action(int? ID)
        {
            TeacherActionModel model = new TeacherActionModel();

            if (ID.HasValue)  //we are trying to edit a record
            {
                var teacher = teacherService.GetTeacherByID(ID.Value);
                string getuser = User.Identity.GetUserId();

                model.ID = teacher.TeacherId;
                model.SubjectId = teacher.SubjectId;
                model.Name = teacher.TeacherName;
                model.User.Id = getuser;
                model.TeacherPictures = teacherService.GetPicturesByTeacherID(teacher.TeacherId);
            }
            model.Subjects = subjectService.GetAllSubjects();
            return PartialView("_Action", model);
        }


        [HttpPost]
        public JsonResult Action(TeacherActionModel model)
        {
            JsonResult json = new JsonResult();

            var result = false;
            List<int> pictureIDs = !string.IsNullOrEmpty(model.PictureIDs) ? model.PictureIDs.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>();
            var pictures = dashboardService.GetPicturesByID(pictureIDs);

            if (model.ID > 0)
            {
                var teacher = teacherService.GetTeacherByID(model.ID);
                string getuser = User.Identity.GetUserId();

                teacher.TeacherId = model.ID;
                teacher.SubjectId = model.SubjectId;
                teacher.TeacherName = model.Name;
                teacher.ApplicationUserID = getuser;
                teacher.TeacherPictures.Clear();
                teacher.TeacherPictures.AddRange(pictures.Select(x => new TeacherPictures() { TeacherId = teacher.TeacherId, PictureID = x.ID }));

                result = teacherService.UpdateTeacher(teacher);
            }
            else
            {
                Teacher teacher = new Teacher();

                teacher.TeacherId = model.ID;
                teacher.SubjectId = model.SubjectId;
                teacher.TeacherName = model.Name;
                string getuser = User.Identity.GetUserId();
                teacher.ApplicationUserID = getuser;

                teacher.TeacherPictures = new List<TeacherPictures>();
                teacher.TeacherPictures.AddRange(pictures.Select(x => new TeacherPictures() { PictureID = x.ID }));

                result = teacherService.SaveTeacher(teacher);
            }


            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on Teachers." };
            }
            return json;
        }


        [HttpGet]
        public ActionResult Delete(int ID)
        {
            SubjectActionModel model = new SubjectActionModel();

            var teacher = teacherService.GetTeacherByID(ID);

            teacher.TeacherId = model.ID;


            return PartialView("_Delete", model);
        }


        [HttpPost]
        public JsonResult Delete(TeacherActionModel model)
        {
            JsonResult json = new JsonResult();

            var result = false;

            var teacher = teacherService.GetTeacherByID(model.ID);



            result = teacherService.DeleteTeacher(teacher);

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
