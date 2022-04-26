using ShoolsLMS.Models;
using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Services
{
    public class SubjectsService
    {
        public IEnumerable<Subject> GetAllSubjects()
        {
            var context = new ApplicationDbContext();
            return context.Subjects.ToList();
        }

        public IEnumerable<Subject> GetAllSubjectsByGrade(int gradeID)
        {
            var context = new ApplicationDbContext();
            return context.Subjects.Where(x => x.GradeId == gradeID).ToList();
        }

        public IEnumerable<Subject> SearchSubject(string searchTerm, int? subjectID, int page, int recordSize)
        {
            var context = new ApplicationDbContext();
            var subjects = context.Subjects.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                subjects = subjects.Where(a => a.SubjectName.ToLower().Contains(searchTerm.ToLower()));
            }

            if (subjectID.HasValue && subjectID.Value > 0)
            {
                subjects = subjects.Where(a => a.SubjectId == subjectID.Value);
            }

            var skip = (page - 1) * recordSize;

            return subjects.OrderBy(t => t.SubjectId == subjectID.Value).Skip(skip).Take(recordSize).ToList();
        }


        public int SearchSubjectCount(string searchTerm, int? subjectID)
        {
            var context = new ApplicationDbContext();
            var subjects = context.Subjects.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                subjects = subjects.Where(a => a.SubjectName.ToLower().Contains(searchTerm.ToLower()));
            }

            if (subjectID.HasValue && subjectID.Value > 0)
            {
                subjects = subjects.Where(a => a.GradeId == subjectID.Value);
            }
            return subjects.Count();
        }

        public Subject GetSubjectByID(int ID)
        {
            var context = new ApplicationDbContext();
            return context.Subjects.Find(ID);
        }
     

        public bool SaveSubject(Subject subject)
        {
            var context = new ApplicationDbContext();

            context.Subjects.Add(subject);
            return context.SaveChanges() > 0;
        }

        public bool UpdateSubject(Subject subject)
        {
            var context = new ApplicationDbContext();
            var existingSubject = context.Subjects.Find(subject.SubjectId);
            context.SubjectPictures.RemoveRange(existingSubject.SubjectPictures);
            context.Entry(existingSubject).CurrentValues.SetValues(subject);
            context.SubjectPictures.AddRange(subject.SubjectPictures);
            return context.SaveChanges() > 0;
        }

        public bool DeleteSubject(Subject subject)
        {
            var context = new ApplicationDbContext();

            var existingSubject = context.Subjects.Find(subject.SubjectId);
            context.SubjectPictures.RemoveRange(existingSubject.SubjectPictures);
            context.Entry(existingSubject).State = System.Data.Entity.EntityState.Deleted;
            return context.SaveChanges() > 0;
        }

        public List<SubjectPictures> GetPicturesBySubjectID(int subjectID)
        {
            var context = new ApplicationDbContext();
            return context.Subjects.Find(subjectID).SubjectPictures.ToList();
        }
    }
}