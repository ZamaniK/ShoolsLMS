using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ShoolsLMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using ShoolsLMS.Models.Data;
using System.Net;
using ShoolsLMS.Models.ViewModels;
using System.IO;
using ShoolsLMS.Infrastructure.Video;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ShoolsLMS.Controllers
{
    public class StudentDashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;
        public StudentDashboardController()
        {
        }
        public StudentDashboardController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

      

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: StudentDashboard
        public ActionResult Index()
        {
            ViewData["user"] = db.Users.ToList().Count();
            ViewData["subject"] = db.Subjects.ToList().Count();
            ViewData["Teacher"] = db.Teachers.ToList().Count();
            return View();
        }

        // GET: StudentDashboard/Dashboard
        public ActionResult Dashboard()
        {
            ViewData["user"] = db.Users.ToList().Count();
            ViewData["subject"] = db.Subjects.ToList().Count();
            ViewData["Teacher"] = db.Teachers.ToList().Count();
            return View();
        }

        public ActionResult Enroll()
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ViewData["Student"] = userManager.FindByNameAsync(User.Identity.Name).Result.Email;
            string getuser = User.Identity.GetUserId();
            
               //// var Subjectid = (from a in db.StudentSubjects
                   //       where a.StudentId == getuser
                     //     select a.SubjectId).SingleOrDefault();
            IEnumerable<StudentSubject> existingSub = db.StudentSubjects.Where(e => e.StudentId == getuser);


            var crs = db.Subjects.Where(e => !existingSub.Any(d => d.SubjectId == e.SubjectId)).Include(c => c.Lessons).ToList();
             

            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName");

            ViewData["StdCrs"] = userManager.FindByNameAsync(User.Identity.Name).Result.Id;
            return View(crs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enroll(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            string getuser = User.Identity.GetUserId();
            Subject subject = db.Subjects.Where(x => x.SubjectId == id).Single();
            var StudentCourse = new StudentSubject();
            StudentCourse.Subject = subject;
            StudentCourse.SubjectId = subject.SubjectId;
            StudentCourse.StudentId = getuser;
            StudentCourse.Status = EnrollStatuss.Accepted;
            db.StudentSubjects.Add(StudentCourse);
            db.SaveChanges();
            return RedirectToAction("Subjects");
        }


        public ActionResult SubjectDetails(int? subjectId)
        {
            if (subjectId == null)
            {
                return HttpNotFound();
            }
            string getuser = User.Identity.GetUserId();
            var coursemodel = db.Subjects.Include(c => c.SubjectName).Include(c => c.User).Include(c => c.Lessons)
                    .Where(e => e.Enrollments
                    .Any(s => s.StudentId == getuser && s.SubjectId == subjectId))
                    .SingleOrDefault();
            return View(coursemodel);
        }
        public ActionResult Subjects()
        {
            var store = User.Identity.GetUserId();
            var getCourse = db.StudentSubjects.Where(x => x.StudentId == store).ToList();
            List<Subject> List = new List<Subject>();
            foreach (var crs in getCourse)
            {
                var course = db.Subjects.Include(c => c.User).Include(c => c.Lessons).Where(y => y.SubjectId == crs.SubjectId).Single();
                List.Add(course);
            }

            return View(List);
        }

        public ActionResult SeeLessons(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = User.Identity.GetUserName();
            var subject = db.Subjects.Find(id);

            if (subject == null)
            {
                return HttpNotFound();
            }

            var model = AutoMapper.Mapper.Map<SeeTeacherVM>(subject);

            return View(model);

        }


        public ActionResult ViewLessons(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = User.Identity.GetUserName();
            var subject = db.Subjects.Include(s => s.Lessons).Where(x => x.SubjectId == id).ToList();


            if (subject == null)
            {
                return HttpNotFound();
            }



            return View(subject);

        }
        [NonAction]
        public void SeeTeachersCodeRemoved(int? lectureId)
        {
            //if (lectureId == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            var user = User.Identity.GetUserName();
            var model = db.Teachers.Find(lectureId);

            //if (model == null)
            //{
            //    return HttpNotFound();
            //}
            var lecturerusername = db.Users.Where(u => u.Id == model.ApplicationUserID).Single();
            var course = db.Subjects.Find(model.SubjectId);
            Dictionary<string, string> files = new Dictionary<string, string>();
            string[] file = Directory.GetFiles(Server.MapPath("~/Content/Uploads/Teachers/" + lecturerusername.UserName + "/" + course.SubjectName), "*.*", SearchOption.AllDirectories);


            foreach (string i in file)
            {
                files.Add(i, Path.GetFileName(i));

            }
            ViewData["filename"] = files;
        }
        public ActionResult WatchLesson(string path)
        {
            //string subjectname = db.Subjects.Where(c => c.SubjectId == lesson.SubjectId).Single().SubjectCode;
            //string uploadpath = Path.Combine(Server.MapPath("~/Content/Uploads/Teachers/"), User.Identity.GetUserName(), subjectname, lesson.LessonName + ".mp4");
            //ViewData["lesson"] = uploadpath;
           // ViewBag.FilePath = Path.Combine(Server.MapPath("~/Content/Uploads/Teachers/"));


            ViewData["path"] = path;
            return PartialView("_VideoPartial", path);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = await db.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
           // string getuser = User.Identity.GetUserId();
           // lesson.User = db.Users.Where(u => u.Id == getuser).Single();
           // db.Lessons.Add(lesson);

            string subjectname = db.Subjects.Where(c => c.SubjectId == lesson.SubjectId).Single().SubjectCode;
            string subjectEducator = db.Subjects.Where(e => e.SubjectId == lesson.SubjectId).Single().User.UserName;
            string uploadpath = Path.Combine(Server.MapPath("~/Content/Uploads/Teachers/" + subjectEducator + "/"), subjectname, lesson.LessonName + ".mp4");
            ViewData["lesson"] = uploadpath;

            ViewBag.FilePath = Path.Combine(Server.MapPath("~/Content/Uploads/Teachers/" + subjectEducator + "/"), subjectname, lesson.FileName);

            return View(lesson);
        }
        public ActionResult DownloadLesson(string path)
        {
            return new VideoResult(path);
        }

        public ActionResult Quiz()
        {
            var store = User.Identity.GetUserId();
            var getCourse = db.StudentSubjects.Where(x => x.StudentId == store).ToList();
            List<Subject> List = new List<Subject>();
            foreach (var crs in getCourse)
            {
                var course = db.Subjects.Include(c => c.SubjectName).Include(c => c.User).Where(y => y.SubjectId == crs.SubjectId).Single();
                List.Add(course);
            }
            
            return View(List);
        }

        public ActionResult SeeQuiz(int? id)
        {
            ViewData["std"] = User.Identity.GetUserId();
            string getuser = User.Identity.GetUserId();

            if (id == null)
            {
                return HttpNotFound();
            }

           // IEnumerable<StudentSubject> existingSub = db.StudentSubjects.Where(e => e.StudentId == getuser);

            var Quizez = db.Quiz.Include(s => s.Students).Where(x => x.SubjectId == id).ToList();
            return View(Quizez);
        }

        public ActionResult TakeQuiz(int quiz)
        {
            var quest = db.Questions.Include(x => x.AnswerChoices).Where(m => m.QuizId == quiz).ToList();
            List<QuizTakingVM> examtakingModels = new List<QuizTakingVM>();

            foreach (var question in quest)
            {
                var examTakingModel = new QuizTakingVM();
                examTakingModel.QuestionId = question.QuestionId;
                examtakingModels.Add(examTakingModel);
            }

            ViewData["quizId"] = quiz;
            return View(examtakingModels);
        }

        [HttpPost]
        public ActionResult TakeQuiz(List<QuizTakingVM> examTakingModels, int? quizId)
        {

            if (quizId == null)
            {
                return HttpNotFound();
            }

            var quiz = new StudentQuiz();
            var store = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(store);
            quiz.Student = userManager.FindByNameAsync(User.Identity.Name).Result;
            quiz.StudentId = User.Identity.GetUserId();

            int totalMarks = db.Quiz.Where(x => x.QuizId == quizId).Single().Score;
            double marksPerQuestion = totalMarks / examTakingModels.Count();

            double obtainedMarks = 0;

            foreach (var examModel in examTakingModels)
            {
                if (examModel.Question.AnswerChoices.Where(ans => ans.AnswerChoiceId == examModel.Choice).SingleOrDefault().IsCorrect == true)
                {
                    obtainedMarks += marksPerQuestion;
                }
            }
            
            quiz.QuizId = quizId.Value;
            quiz.Quiz = db.Quiz.Where(x => x.QuizId == quizId).Single();
            quiz.Marks = obtainedMarks;

            db.StudentQuiz.Add(quiz);
            db.SaveChanges();

            ViewData["marks"] = obtainedMarks;

            return RedirectToAction("QuizHistory");
        }

        public ActionResult QuizHistory()
        {
            var store = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(store);
            var student = userManager.FindByNameAsync(User.Identity.Name).Result;

            var quiz = db.StudentQuiz.Where(sid => sid.StudentId == student.Id).ToList();

            return View(quiz);

        }

        [HttpPost]
        public ActionResult CalculateMarks(int[] Choices, List<Question> question, int quizId)
        {
            var quiz = new StudentQuiz();
            var store = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(store);
            quiz.Student = userManager.FindByNameAsync(User.Identity.Name).Result;
            quiz.StudentId = User.Identity.GetUserId();

            int i = 0;

            int totalMarks = db.Quiz.Where(x => x.QuizId == quizId).Single().Score;
            double marksPerQuestion = totalMarks / question.Count();
            double obtainedMarks = 0;

            foreach (var item in question)
            {
                var q = db.Questions.Include(y => y.AnswerChoices).Where(x => x.QuestionId == item.QuestionId).Single();
                foreach (var choice in q.AnswerChoices)
                {
                    if (choice.IsCorrect == true)
                    {
                        if (Choices[i] == choice.AnswerChoiceId)
                        {
                            obtainedMarks += marksPerQuestion;

                        }

                    }

                }
                i++;
            }
            quiz.QuizId = quizId;
            quiz.Quiz = db.Quiz.Where(x => x.QuizId == quizId).Single();
            quiz.Marks = obtainedMarks;
            db.StudentQuiz.Add(quiz);
            db.SaveChanges();
            ViewData["marks"] = obtainedMarks;
            return View();
        }

        public ActionResult Assignments()
        {
            var store = User.Identity.GetUserId();
            var getCourse = db.StudentSubjects.Where(x => x.StudentId == store).ToList();
            List<Subject> List = new List<Subject>();
            foreach (var crs in getCourse)
            {
                var course = db.Subjects.Include(p =>p.Assignments).Include(c => c.User).Where(y => y.SubjectId == crs.SubjectId).Single();
                List.Add(course);
            }

            return View(List);

        }

        public ActionResult SeeAssignment(int id)
        {
            //var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            //var userManager = new UserManager<ApplicationUser>(store);
            //ViewData["Student"] = userManager.FindByNameAsync(User.Identity.Name).Result;
            ApplicationDbContext db = new ApplicationDbContext();

            ViewData["std"] = User.Identity.GetUserId();


            var Assignment = db.Subjects.Include(s => s.Assignments).Where(x => x.SubjectId == id).ToList();
            return View(Assignment);
        }



        public ActionResult Books()
        {
            var store = User.Identity.GetUserId();
            var getCourse = db.StudentSubjects.Where(x => x.StudentId == store).ToList();
            List<Subject> List = new List<Subject>();
            foreach (var crs in getCourse)
            {
                var course = db.Subjects.Include(p => p.Assignments).Include(c => c.User).Where(y => y.SubjectId == crs.SubjectId).Single();
                List.Add(course);
            }

            return View(List);

        }

        public ActionResult SeeEbooks(int id)
        {

            ApplicationDbContext db = new ApplicationDbContext();



            var Ebooks = db.Subjects.Include(s => s.Books).Where(x => x.SubjectId == id).ToList();
            return View(Ebooks);
        }

        public ActionResult SeeQuestionPapers(int id)
        {

            ApplicationDbContext db = new ApplicationDbContext();



            var Papers = db.Subjects.Include(s => s.Papers).Where(x => x.SubjectId == id).ToList();
            return View(Papers);
        }

        //PDF Action

        public FileResult OpenPDF(string fileName)
        {
            return File(Server.MapPath("/Content/Uploads/Teachers/" + fileName), "application/pdf");
        }

        public FileResult DownloadPDF(string fileName)
        {
            
            string filepath = Server.MapPath("/Content/Uploads/Teachers/" + fileName);
            byte[] pdfByte = GetBytesFromFile(filepath);
            return File(pdfByte, "application/pdf", fileName);
        }

        public FileResult DownloadBook(string fileName)
        {

            //if (model == null)
            //{
            //    return HttpNotFound();
            //}
            // var lecturerusername = db.Users.Where(u => u.Id == model.ApplicationUserID).Single();
            //var course = db.Subjects.Find(model.SubjectId);
            //Dictionary<string, string> files = new Dictionary<string, string>();
            //string[] file = Directory.GetFiles(Server.MapPath("~/Content/Uploads/Teachers/" + lecturerusername.UserName + "/" + course.SubjectName), "*.*", SearchOption.AllDirectories);

            string filepath = Server.MapPath("/Content/Uploads/Teachers/" + fileName);
            byte[] pdfByte = GetBytesFromFile(filepath);
            return File(pdfByte, "application/pdf", fileName);
        }
        public FileResult DisplayBook(string fileName)
        {
            string filepath = Server.MapPath("/Content/Uploads/Teachers/" + fileName);
            byte[] pdfByte = GetBytesFromFile(filepath);
            return File(pdfByte, "application/pdf");
        }
        public FileResult DisplayPDF(string fileName)
        {
            string filepath = Server.MapPath("/Content/Uploads/Teachers/" + fileName);
            byte[] pdfByte = GetBytesFromFile(filepath);
            return File(pdfByte, "application/pdf");
        }
        public PartialViewResult PDFPartialView()
        {
            return PartialView();
        }



        public byte[] GetBytesFromFile(string filepath)
        {

            //this method is limited to 2^32 byte files (4.2 GB)
            FileStream fs = null;
            try
            {
                fs = System.IO.File.OpenRead(filepath);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                return bytes;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }



        public FileResult VideoDownload(string FileName)
        {
            string contentType = string.Empty;

            if (FileName.Contains(".mp4"))
            {
                contentType = "video/mp4";
            }

            else if (FileName.Contains(".mkv"))
            {
                contentType = "video/mp4";
            }

            return File(FileName, contentType);
        }



        [HttpGet]
        public ActionResult GetPDF(string fileName)
        {

            string filePath = Path.Combine(@"~/Uploads/Teachers/" + fileName);
            return File(filePath, "application/pdf");
        }

        public FileContentResult File(string fileName) 
        {
            var FullPathToFile = @"Uploads\Teachers\Zamanikat100@gmail.com\mth102\"+ fileName;
            var mimeType = "application/pdf";
            var fileContents = System.IO.File.ReadAllBytes(FullPathToFile);

            return new FileContentResult(fileContents, mimeType);
        }
        public ActionResult Download()
        {

            var document = new Assignment();


            var cd = new System.Net.Mime.ContentDisposition

            //var cd = new ContentDispositionHeaderValue("attachment")
            {
                //for example foo.bak
                FileName = document.FileName,

                //always prompt the user for downloading, set to true if you want the browser to try to show the file inline

                Inline = false,

            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            //Response.Headers.Add(HeaderNames.ContentDisposition, cd.ToString());
            return File(document.Data, document.ContentType);
        }


        public ActionResult PracticeCode()
        {
            return View();
        }


        public ActionResult SendRating(string r, string id, string url)
        {
            try
            {
                //var context = new ApplicationDbContext();

                string currentUserId = User.Identity.GetUserId();

                int autoid = 0;
                Int16 thisVote = 0;
                Int16.TryParse(r, out thisVote);
                int.TryParse(id, out autoid);

                if (!User.Identity.IsAuthenticated)
                {
                    return Json("Not authenticated!");
                }

                switch (r)
                {

                    case "5":
                    case "4":
                    case "3":
                    case "2":
                    case "1":

                        var userid = User.Identity.GetUserId();
                        var rating = db.Ratings.Where(o => o.SubjectId == autoid && o.UserId == userid).SingleOrDefault();

                        if (rating != null)
                        {
                            HttpCookie Cookie = new HttpCookie(url, "true");
                            Response.Cookies.Add(Cookie);
                            return Json("<br/> You havr already rate ,Thanks!...", JsonRequestBehavior.AllowGet);

                        }
                        var rating2 = new Rating();
                        var store = new UserStore<ApplicationUser>(db);
                        var userManager = new UserManager<ApplicationUser>(store);
                        var user = userManager.FindByName(User.Identity.Name);


                        rating2.User = user;
                        rating2.UserId = userid;
                        rating2.SubjectId = autoid;
                        rating2.Subject = db.Subjects.Where(x => x.SubjectId == autoid).Single();

                        rating2.stars = thisVote;
                        db.Ratings.Add(rating2);
                        if (db.SaveChanges() > 0)
                        {
                            HttpCookie cookie = new HttpCookie(url, "true");
                            Response.Cookies.Add(cookie);

                            return Json("<br />You rated " + r + " star(s), thanks !", JsonRequestBehavior.AllowGet);

                        }


                        break;
                    default:
                        break;
                }
                return Json("Rating Failed!", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HttpCookie cookie = new HttpCookie(url, "true");
                Response.Cookies.Add(cookie);

                return Json(ex.Message, JsonRequestBehavior.AllowGet);

                throw;
            }
        }

        public ActionResult VoteNow(int id)
        {
            ViewData["id"] = id;
            return View();
        }

       
    }
}