using ShoolsLMS.DataRepos.Reposisoty;
using ShoolsLMS.Models.Data;
using ShoolsLMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShoolsLMS.Areas.Dashboard.Controllers
{
    public class GradeController : Controller
    {
        protected ISchoolLMSData Data { get; private set; }
        public GradeController()
        {
        }
        public GradeController(ISchoolLMSData data)
        {
            this.Data = data;
        }

        

        // GET: Grades
        [AllowAnonymous]
        public ActionResult Index()
        {
            var model = AutoMapper.Mapper.Map<IEnumerable<GradeViewModel>>(this.Data.Grades.All());
            return View(model);
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade grade = this.Data.Grades.Find(id);

            if (grade == null)
            {
                return HttpNotFound();
            }

            var model = AutoMapper.Mapper.Map<GradeViewModel>(grade);
            return View(model);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            //ViewData["Grade"] = new SelectList(this.Data.Grades.All().ToList(), "GradeId", "GradeName", "");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GradeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var grade = AutoMapper.Mapper.Map<Grade>(model);
                this.Data.Grades.Add(grade);
                this.Data.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Grade category = this.Data.Grades.Find(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            var model = AutoMapper.Mapper.Map<GradeViewModel>(category);
            return View(model);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GradeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = AutoMapper.Mapper.Map<Grade>(model);
                this.Data.Grades.Update(category);
                this.Data.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade category = this.Data.Grades.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            var model = AutoMapper.Mapper.Map<GradeViewModel>(category);
            return View(model);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Grade category = this.Data.Grades.Find(id);
            this.Data.Grades.Delete(category);
            this.Data.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}