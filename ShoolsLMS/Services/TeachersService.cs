using ShoolsLMS.Models;
using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Services
{
    public class TeachersService
    {
        public IEnumerable<Teacher> GetAllTeachers()
        {
            var context = new ApplicationDbContext();
            return context.Teachers.ToList();
        }

        public IEnumerable<Teacher> GetAllTeachersBySubject(int subjectID)
        {
            var context = new ApplicationDbContext();
            return context.Teachers.Where(x => x.SubjectId == subjectID).ToList();
        }

        public IEnumerable<Teacher> SearchTeacher(string searchTerm, int? teacherID, int page, int recordSize)
        {
            var context = new ApplicationDbContext();
            var teachers = context.Teachers.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                teachers = teachers.Where(a => a.TeacherName.ToLower().Contains(searchTerm.ToLower()));
            }

            if (teacherID.HasValue && teacherID.Value > 0)
            {
                teachers = teachers.Where(a => a.TeacherId == teacherID.Value);
            }

            var skip = (page - 1) * recordSize;

            return teachers.OrderBy(t => t.TeacherId == teacherID.Value).Skip(skip).Take(recordSize).ToList();
        }


        public int SearchTeacherCount(string searchTerm, int? teacherID)
        {
            var context = new ApplicationDbContext();
            var teachers = context.Teachers.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                teachers = teachers.Where(a => a.TeacherName.ToLower().Contains(searchTerm.ToLower()));
            }

            if (teacherID.HasValue && teacherID.Value > 0)
            {
                teachers = teachers.Where(a => a.SubjectId == teacherID.Value);
            }
            return teachers.Count();
        }

        public Teacher GetTeacherByID(int ID)
        {
            var context = new ApplicationDbContext();
            return context.Teachers.Find(ID);
        }


        public bool SaveTeacher(Teacher teacher)
        {

            var context = new ApplicationDbContext();

            context.Teachers.Add(teacher);
            return context.SaveChanges() > 0;
        }

        public bool UpdateTeacher(Teacher teacher)
        {
            var context = new ApplicationDbContext();
            var existingTeachers = context.Teachers.Find(teacher.TeacherId);
            context.teacherPictures.RemoveRange(existingTeachers.TeacherPictures);
            context.Entry(existingTeachers).CurrentValues.SetValues(teacher);
            context.teacherPictures.AddRange(teacher.TeacherPictures);
            return context.SaveChanges() > 0;
        }

        public bool DeleteTeacher(Teacher teacher)
        {
            var context = new ApplicationDbContext();

            var existingTeachers = context.Teachers.Find(teacher.TeacherId);
            context.teacherPictures.RemoveRange(existingTeachers.TeacherPictures);
            context.Entry(existingTeachers).State = System.Data.Entity.EntityState.Deleted;
            return context.SaveChanges() > 0;
        }

        public List<TeacherPictures> GetPicturesByTeacherID(int teacherID)
        {
            var context = new ApplicationDbContext();
            return context.Teachers.Find(teacherID).TeacherPictures.ToList();
        }
    }
}