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
    public class TeacherQuizsController : Controller
    {
        QuizService quizService = new QuizService();
        SubjectsService subjectsService = new SubjectsService();

        public ActionResult Index(string searchTerm, int? subjectID, int? page)
        {
            QuizListingModel model = new QuizListingModel();

            int recordSize = 15;
            page = page ?? 1;

            model.SearchTerm = searchTerm;
            model.SubjectID = subjectID;

            model.Quizs = quizService.SearchBooks(searchTerm, subjectID, page.Value, recordSize);

            model.Subjects = subjectsService.GetAllSubjects();

            var totalRecords = subjectsService.SearchSubjectCount(searchTerm, subjectID);
            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);
        }

        [HttpGet]
        public ActionResult Action(int? ID)
        {
            QuizActionModel model = new QuizActionModel();

            if (ID.HasValue)  //we are trying to edit a record
            {
                var quiz = quizService.GetQuizByID(ID.Value);

                quiz.SubjectId = model.SubjectId;
                quiz.QuizName = model.QuizName;
                quiz.StartTime = model.StartTime;

                quiz.Score = model.Score;

            }
            model.Subjects = subjectsService.GetAllSubjects();
            return PartialView("_Action", model);
        }


        [HttpPost]
        public JsonResult Action(QuizActionModel model)
        {
            JsonResult json = new JsonResult();

            var result = false;



            if (model.ID > 0)
            {
                var quiz = quizService.GetQuizByID(model.ID);


                quiz.SubjectId = model.SubjectId;
                quiz.QuizName = model.QuizName;
                quiz.StartTime = DateTime.Today;

                quiz.Score = model.Score;

                result = quizService.UpdateQuiz(quiz);
            }
            else
            {
                Quiz quiz = new Quiz();

                string getuser = User.Identity.GetUserId();

                quiz.SubjectId = model.SubjectId;
                quiz.QuizName = model.QuizName;
                quiz.StartTime = DateTime.Today;

                quiz.Score = model.Score;

                result = quizService.SaveQuiz(quiz);


            }


            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on Quiz." };
            }
            return json;
        }


        [HttpGet]
        public ActionResult Delete(int ID)
        {
            QuizActionModel model = new QuizActionModel();

            var book = quizService.GetQuizByID(ID);

            model.ID = book.QuizId;


            return PartialView("_Delete", model);
        }


        [HttpPost]
        public JsonResult Delete(QuizActionModel model)
        {
            JsonResult json = new JsonResult();

            var result = false;

            var quiz = quizService.GetQuizByID(model.ID);



            result = quizService.DeleteGrade(quiz);

            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on Quiz." };
            }
            return json;
        }
    }
}