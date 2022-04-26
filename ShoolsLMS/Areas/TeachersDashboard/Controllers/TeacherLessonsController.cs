using Microsoft.AspNet.Identity.Owin;
using ShoolsLMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.Net;
using ShoolsLMS.Models.Data;
using Microsoft.AspNet.Identity;
using System.IO;
using ShoolsLMS.Infrastructure.FileHelpers;

namespace ShoolsLMS.Areas.TeachersDashboard.Controllers
{
    public class TeacherLessonsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public TeacherLessonsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public TeacherLessonsController()
        {
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



        // GET: Lessons
        public async Task<ActionResult> Index()
        {
            ApplicationUser user = GetUser();
            var lessons = db.Lessons.Where(l => l.User.Id == user.Id).Include(l => l.Subject);
            return View(await lessons.ToListAsync());
        }

        // GET: Lectures/Details/5
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
            string getuser = User.Identity.GetUserId();
            lesson.User = db.Users.Where(u => u.Id == getuser).Single();
            db.Lessons.Add(lesson);

            string subjectname = db.Subjects.Where(c => c.SubjectId == lesson.SubjectId).Single().SubjectCode;
            string uploadpath = Path.Combine(Server.MapPath("~/Content/Uploads/Teachers/"), User.Identity.GetUserName(), subjectname, lesson.LessonName + ".mp4");
            ViewData["lesson"] = uploadpath;

            ViewBag.FilePath = Path.Combine(Server.MapPath("~/Content/Uploads/Teachers/"), User.Identity.GetUserName(), subjectname, lesson.FileName);

            return View(lesson);
        }

        // GET: Lectures/Create
        public ActionResult Create()
        {
            string getuser = User.Identity.GetUserId();

            ViewBag.SubjectId = new SelectList(db.Subjects.Where(c => c.User.Id == getuser).ToList(), "SubjectId", "SubjectName");
            //ViewBag.ApplicationUserID = new SelectList(getuser, "Id", "Email");
            return View();
        }

        // POST: Lectures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Lesson teacher, HttpPostedFileBase upload)
        {
            string getuser = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                teacher.User = db.Users.Where(u => u.Id == getuser).Single();

                if (upload.ContentLength > 0)
                {
                    string subjectname = db.Subjects.Where(c => c.SubjectId == teacher.SubjectId).Single().SubjectCode;
                    string uploadedFileName = FileUtils.UploadFile(upload, User.Identity.GetUserName(), subjectname);

                    teacher.FileName = uploadedFileName;

                    db.Lessons.Add(teacher);
                    await db.SaveChangesAsync();
                    //string uploadDirectoryPath = Path.Combine(Server.MapPath("~/Content/Uploads/Lecturers/"), User.Identity.GetUserName(), coursename);
                    //if (!Directory.Exists(uploadDirectoryPath))
                    //{
                    //    Directory.CreateDirectory(uploadDirectoryPath);
                    //}
                    //string uploadpath = Path.Combine(Server.MapPath("~/Content/Uploads/Lecturers/"), User.Identity.GetUserName(), coursename, lecture.LectureName + ".mp4");
                    //upload.SaveAs(uploadpath);
                }

                return RedirectToAction("Index");
            }

            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName", teacher.SubjectId);
            //ViewBag.ApplicationUserID = new SelectList(db.ApplicationUsers, "Id", "Email", lecture.ApplicationUserID);
            return View(teacher);
        }

        // GET: Lectures/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson teacher = await db.Lessons.FindAsync(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName", teacher.SubjectId);
            //ViewBag.ApplicationUserID = new SelectList(db.ApplicationUsers, "Id", "Email", lecture.ApplicationUserID);
            return View(teacher);
        }

        // POST: Lectures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Lesson teacher, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload.ContentLength > 0)
                {
                    string subjectname = db.Subjects.Where(c => c.SubjectId == teacher.SubjectId).Single().SubjectCode;
                    //string uploadpath = Path.Combine(Server.MapPath("~/Content/Uploads/Lecturers/"), User.Identity.GetUserName(), coursename, lecture.LectureName + ".mp4");
                    //upload.SaveAs(uploadpath);
                    string uploadedFileName = FileUtils.UploadFile(upload, User.Identity.GetUserName(), subjectname);

                    teacher.FileName = uploadedFileName;
                }

                db.Entry(teacher).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName", teacher.SubjectId);
            return View(teacher);
        }

        // GET: Lectures/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson teacher = await db.Lessons.FindAsync(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Lectures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Lesson teacher = await db.Lessons.FindAsync(id);
            db.Lessons.Remove(teacher);
            string subjectname = db.Subjects.Where(c => c.SubjectId == teacher.SubjectId).Single().SubjectCode;
            string uploadpath = Path.Combine(Server.MapPath("~/Content/Uploads/Teachers/"), User.Identity.GetUserName(), subjectname, teacher.LessonName + ".mp4");
            System.IO.File.Delete(uploadpath);

            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [NonAction]
        public virtual ApplicationUser GetUser()
        {
            string username = this.User.Identity.Name;
            var user = UserManager.FindByName(username);
            return user;
        }
        public FileResult Download(string FileName)
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
    }
}