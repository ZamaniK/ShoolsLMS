using Microsoft.AspNet.Identity;
using ShoolsLMS.Models;
using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoolsLMS.Infrastructure.FileHelpers;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using ShoolsLMS.Areas.Dashboard.ViewModels;
using ShoolsLMS.Services;

namespace ShoolsLMS.Areas.Dashboard.Controllers
{
    public class AssignmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        SubjectsService subjectsService = new SubjectsService();
        AssignmentServive assignmentServive = new AssignmentServive();


        public AssignmentsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public AssignmentsController()
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
        public ActionResult Index(string searchTerm, int? subjectID, int? page)
        {
            AssignmentListingModel model = new AssignmentListingModel();
            string getuser = User.Identity.GetUserId();

            int recordSize = 15;
            page = page ?? 1;

            model.SearchTerm = searchTerm;
            model.SubjectID = subjectID;

            model.Assignments = SearchAssignment(searchTerm, subjectID, page.Value, recordSize);

            model.Subjects = GetAllSubjects().Where(e => e.ApplicationUserID == getuser);

            var totalRecords = subjectsService.SearchSubjectCount(searchTerm, subjectID);
            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);
        }
        public IEnumerable<Subject> GetAllSubjects()
        {
            var context = new ApplicationDbContext();
            return context.Subjects.ToList();
        }



        public IEnumerable<Assignment> SearchAssignment(string searchTerm, int? assignID, int page, int recordSize)
        {
            var context = new ApplicationDbContext();
            var assignment = context.Assignment.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                assignment = assignment.Where(a => a.AssignmentName.ToLower().Contains(searchTerm.ToLower()));
            }

            if (assignID.HasValue && assignID.Value > 0)
            {
                assignment = assignment.Where(a => a.AssignmentId == assignID.Value);
            }

            var skip = (page - 1) * recordSize;

            return assignment.OrderBy(t => t.AssignmentId == assignID.Value).Skip(skip).Take(recordSize).ToList();
        }
        /*[AllowAnonymous]
        public ActionResult Index()
        {
            string getuser = User.Identity.GetUserId();
            var assignment = db.Assignment.Where(e => e.Subject.ApplicationUserID == getuser).Include(a => a.Subject);
            return View(assignment.ToList());
        }*/

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