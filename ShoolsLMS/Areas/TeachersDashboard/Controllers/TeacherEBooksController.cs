using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ShoolsLMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.Net;
using ShoolsLMS.Models.Data;
using ShoolsLMS.Infrastructure.FileHelpers;

namespace ShoolsLMS.Areas.TeachersDashboard.Controllers
{
    public class TeacherEBooksController : Controller
    {
        private ApplicationUserManager _userManager;

        public TeacherEBooksController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public TeacherEBooksController()
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
        // GET: Dashboard/eBooks
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            ApplicationUser user = GetUser();
            string getuser = User.Identity.GetUserId();

            var books = db.Books.Include(a => a.Subject).Where(a => a.Subject.ApplicationUserID == getuser);
            return View(books.ToList());
        }

        // GET: QuestionPapers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            string subjectname = db.Subjects.Where(c => c.SubjectId == book.SubjectId).Single().SubjectCode;
            ViewBag.FilePath = Server.MapPath(FileUtils.UPLOAD_PATH + "/" + book.FileName);

            return View(book);
        }

        // GET: QuestionPapers/Create
        public ActionResult Create()
        {
            string userid = User.Identity.GetUserId();

            ViewBag.SubjectId = new SelectList(db.Subjects.Where(x => x.User.Id == userid), "SubjectId", "SubjectName");
            return View();
        }

        // POST: QuestionPapers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book, HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var subject = db.Subjects.Find(book.SubjectId);

                if (subject.Lessons.Count() > 0)
                {
                    string subjectname = db.Subjects.Where(c => c.SubjectId == book.SubjectId).Single().SubjectCode;
                    //string ext = Path.GetExtension(upload.FileName);
                    //string uploadpath = Path.Combine(Server.MapPath("~/Content/Uploads/Lecturers/"), User.Identity.GetUserName(), coursename, assignment.AssignmentName + ext);
                    //upload.SaveAs(uploadpath);
                    string uploadedFileName = FileUtils.UploadFile(upload);

                    book.FileName = uploadedFileName;

                    db.Books.Add(book);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            TempData["Error"] = "Unable to Add eBook";
            string userid = User.Identity.GetUserId();
            ViewBag.SubjectId = new SelectList(db.Subjects.Where(x => x.User.Id == userid), "SubjectId", "SubjectName");
            return View(book);
        }

        // GET: Assignments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            string userid = User.Identity.GetUserId();
            string subjectname = db.Subjects.Where(c => c.SubjectId == book.SubjectId).Single().SubjectCode;

            ViewBag.SubjectId = new SelectList(db.Subjects.Where(x => x.User.Id == userid), "SubjectId", "SubjectName");
            return View(book);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book book, HttpPostedFileBase upload)
        {
            string subjectname = db.Subjects.Where(c => c.SubjectId == book.SubjectId).Single().SubjectCode;

            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    string uploadedFileName = FileUtils.UploadFile(upload, User.Identity.GetUserName(), subjectname);

                    book.FileName = uploadedFileName;
                }
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName");
            return View(book);
        }

        // GET: Assignments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [NonAction]
        public virtual ApplicationUser GetUser()
        {
            string username = this.User.Identity.GetUserId();
            var user = UserManager.FindById(username);
            return user;
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