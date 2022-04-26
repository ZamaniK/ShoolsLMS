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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ShoolsLMS.Areas.Dashboard.ViewModels;
using ShoolsLMS.Services;

namespace ShoolsLMS.Areas.Dashboard.Controllers
{
    public class QuestionPapersController : Controller
    {
        // GET: Dashboard/QuestionPapers
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public QuestionPapersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public QuestionPapersController()
        {
        }
        PapersService paperService = new PapersService();
        SubjectsService subjectsService = new SubjectsService();
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
        [AllowAnonymous]
       // public ActionResult Index()
       // {
       //     string getuser = User.Identity.GetUserId();
       //     var paper = db.Papers.Where(e => e.Subject.ApplicationUserID == getuser).Include(a => a.Subject);
       //     return View(paper.ToList());
       // }
       public ActionResult Index(string searchTerm, int? subjectID, int? page)
        {
            PapersListingModel model = new PapersListingModel();

            int recordSize = 15;
            page = page ?? 1;

            model.SearchTerm = searchTerm;
            model.SubjectID = subjectID;

            model.Papers = paperService.SearchPapers(searchTerm, subjectID, page.Value, recordSize);

            model.Subjects = subjectsService.GetAllSubjects();

            var totalRecords = subjectsService.SearchSubjectCount(searchTerm, subjectID);
            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);
        }

        // GET: QuestionPapers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paper paper = db.Papers.Find(id);
            if (paper == null)
            {
                return HttpNotFound();
            }
            string subjectname = db.Subjects.Where(c => c.SubjectId == paper.SubjectId).Single().SubjectCode;
            ViewBag.FilePath = Server.MapPath(FileUtils.UPLOAD_PATH +  "/" + paper.FileName);

            return View(paper);
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
        public ActionResult Create(Paper paper, HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var subject = db.Subjects.Find(paper.SubjectId);

                if (subject.Lessons.Count() > 0)
                {
                    string subjectname = db.Subjects.Where(c => c.SubjectId == paper.SubjectId).Single().SubjectCode;
                    //string ext = Path.GetExtension(upload.FileName);
                    //string uploadpath = Path.Combine(Server.MapPath("~/Content/Uploads/Lecturers/"), User.Identity.GetUserName(), coursename, assignment.AssignmentName + ext);
                    //upload.SaveAs(uploadpath);
                    string uploadedFileName = FileUtils.UploadFile(upload);

                    paper.FileName = uploadedFileName;

                    db.Papers.Add(paper);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            TempData["Error"] = "Unable to Add Question Paper";
            string userid = User.Identity.GetUserId();
            ViewBag.SubjectId = new SelectList(db.Subjects.Where(x => x.User.Id == userid), "SubjectId", "SubjectName");
            return View(paper);
        }

        // GET: Assignments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paper paper = db.Papers.Find(id);
            if (paper == null)
            {
                return HttpNotFound();
            }
            string userid = User.Identity.GetUserId();
            string subjectname = db.Subjects.Where(c => c.SubjectId == paper.SubjectId).Single().SubjectCode;

            ViewBag.SubjectId = new SelectList(db.Subjects.Where(x => x.User.Id == userid), "SubjectId", "SubjectName");
            return View(paper);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Paper paper, HttpPostedFileBase upload)
        {
            string subjectname = db.Subjects.Where(c => c.SubjectId == paper.SubjectId).Single().SubjectCode;

            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    string uploadedFileName = FileUtils.UploadFile(upload);

                    paper.FileName = uploadedFileName;
                }
                db.Entry(paper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName");
            return View(paper);
        }

        // GET: Assignments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paper paper = db.Papers.Find(id);
            if (paper == null)
            {
                return HttpNotFound();
            }
            return View(paper);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Paper paper = db.Papers.Find(id);
            db.Papers.Remove(paper);
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