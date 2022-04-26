using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ShoolsLMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Net;
using ShoolsLMS.Models.Data;
using ShoolsLMS.Infrastructure.FileHelpers;

namespace ShoolsLMS.Areas.TeachersDashboard.Controllers
{
    public class TeacherAssignmentController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public TeacherAssignmentController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public TeacherAssignmentController()
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

        [NonAction]
        public virtual ApplicationUser GetUser()
        {
            string username = this.User.Identity.Name;
            var user = UserManager.FindByName(username);
            return user;
        }
        // GET: Assignments
        [AllowAnonymous]
        public ActionResult Index()
        {
            string getuser = User.Identity.GetUserId();
            var assignment = db.Assignment.Where(e => e.Subject.ApplicationUserID == getuser).Include(a => a.Subject);
            return View(assignment.ToList());
        }

        // GET: Assignments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignment.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            string coursename = db.Subjects.Where(c => c.SubjectId == assignment.SubjectId).Single().SubjectCode;
            ViewBag.FilePath = Server.MapPath(FileUtils.UPLOAD_PATH + "/" + assignment.FileName);

            return View(assignment);
        }

        // GET: Assignments/Create
        public ActionResult Create()
        {
            string userid = User.Identity.GetUserId();

            ViewBag.SubjectId = new SelectList(db.Subjects.Where(x => x.User.Id == userid), "SubjectId", "SubjectName");
            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Assignment assignment, HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var course = db.Subjects.Find(assignment.SubjectId);

                if (course.Lessons.Count() > 0)
                {
                    string coursename = db.Subjects.Where(c => c.SubjectId == assignment.SubjectId).Single().SubjectCode;
                    //string ext = Path.GetExtension(upload.FileName);
                    //string uploadpath = Path.Combine(Server.MapPath("~/Content/Uploads/Lecturers/"), User.Identity.GetUserName(), coursename, assignment.AssignmentName + ext);
                    //upload.SaveAs(uploadpath);
                    string uploadedFileName = FileUtils.UploadFile(upload);

                    assignment.FileName = uploadedFileName;

                    db.Assignment.Add(assignment);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            TempData["Error"] = "Unable to Add Assignments";
            string userid = User.Identity.GetUserId();
            ViewBag.SubjectId = new SelectList(db.Subjects.Where(x => x.User.Id == userid), "SubjectId", "SubjectName");
            return View(assignment);
        }

        // GET: Assignments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignment.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            string userid = User.Identity.GetUserId();
            string coursename = db.Subjects.Where(c => c.SubjectId == assignment.SubjectId).Single().SubjectCode;

            ViewBag.SubjectId = new SelectList(db.Subjects.Where(x => x.User.Id == userid), "SubjectId", "SubjectName");
            return View(assignment);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Assignment assignment, HttpPostedFileBase upload)
        {
            string coursename = db.Subjects.Where(c => c.SubjectId == assignment.SubjectId).Single().SubjectCode;

            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    string uploadedFileName = FileUtils.UploadFile(upload);

                    assignment.FileName = uploadedFileName;
                }
                db.Entry(assignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName");
            return View(assignment);
        }

        // GET: Assignments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignment.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Assignment assignment = db.Assignment.Find(id);
            db.Assignment.Remove(assignment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public FileResult Download(string FileName)
        {
            string contentType = string.Empty;

            if (FileName.Contains(".pdf"))
            {
                contentType = "application/pdf";
            }

            else if (FileName.Contains(".docx"))
            {
                contentType = "application/docx";
            }

            return File(FileName, contentType);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}