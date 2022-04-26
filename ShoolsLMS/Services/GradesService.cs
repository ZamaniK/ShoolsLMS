using ShoolsLMS.Models;
using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Services
{
    public class GradesService
    {
        public IEnumerable<Grade> GetAllGrades()
        {
            var context = new ApplicationDbContext();
            return context.Grades.ToList();
        }

        public IEnumerable<Grade> SearchGrades(string searchTerm, int page, int recordSize)
        {
            var context = new ApplicationDbContext();
            var grades = context.Grades.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                grades = grades.Where(a => a.GradeName.ToLower().Contains(searchTerm.ToLower()));
            }

            var skip = (page - 1) * recordSize;

            return grades.OrderBy(t => t.GradeId).Skip(skip).Take(recordSize).ToList();
        }
        public int SearchGradeCount(string searchTerm)
        {
            var context = new ApplicationDbContext();
            var grades = context.Grades.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                grades = grades.Where(a => a.GradeName.ToLower().Contains(searchTerm.ToLower()));
            }

            
            return grades.Count();
        }

        public Grade GetGradeByID(int ID)
        {
            var context = new ApplicationDbContext();

            return context.Grades.Find(ID);
        }

        public bool SaveGrade(Grade grade)
        {
            var context = new ApplicationDbContext();

            context.Grades.Add(grade);
            return context.SaveChanges() > 0;
        }

        public bool UpdateGrade(Grade grade)
        {
            var context = new ApplicationDbContext();

            var existingGrade = context.Grades.Find(grade.GradeId);
            context.Entry(existingGrade).State = System.Data.Entity.EntityState.Modified;
            return context.SaveChanges() > 0;
        }

        public bool DeleteGrade(Grade grade)
        {
            var context = new ApplicationDbContext();

            var existingGrade = context.Grades.Find(grade.GradeId);
            context.Entry(existingGrade).State = System.Data.Entity.EntityState.Deleted;
            return context.SaveChanges() > 0;
        }
    }
}