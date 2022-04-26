using ShoolsLMS.Models;
using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Services
{
    public class QuizService
    {
        public IEnumerable<Quiz> GetAllQuizs()
        {
            var context = new ApplicationDbContext();
            return context.Quiz.ToList();
        }

        public IEnumerable<Quiz> SearchBooks(string searchTerm, int? QuizID, int page, int recordSize)
        {
            var context = new ApplicationDbContext();
            var quizs = context.Quiz.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                quizs = quizs.Where(a => a.QuizName.ToLower().Contains(searchTerm.ToLower()));
            }
            if (QuizID.HasValue && QuizID.Value > 0)
            {
                quizs = quizs.Where(a => a.QuizId == QuizID.Value);
            }

            var skip = (page - 1) * recordSize;

            return quizs.OrderBy(t => t.QuizId == QuizID.Value).Skip(skip).Take(recordSize).ToList();
        }

        public Quiz GetQuizByID(int ID)
        {
            var context = new ApplicationDbContext();

            return context.Quiz.Find(ID);
        }
        public IEnumerable<Quiz> GetAllQuizsBySubject(int subjectID)
        {
            var context = new ApplicationDbContext();
            return context.Quiz.Where(x => x.SubjectId == subjectID).ToList();
        }


        public bool SaveQuiz(Quiz quiz)
        {
            var context = new ApplicationDbContext();

            context.Quiz.Add(quiz);
            return context.SaveChanges() > 0;
        }

        public bool UpdateQuiz(Quiz quiz)
        {
            var context = new ApplicationDbContext();

            var existingQuiz = context.Quiz.Find(quiz.QuizId);
            context.Entry(existingQuiz).State = System.Data.Entity.EntityState.Modified;
            return context.SaveChanges() > 0;
        }

        public bool DeleteGrade(Quiz quiz)
        {
            var context = new ApplicationDbContext();

            var existingQuiz = context.Quiz.Find(quiz.QuizId);
            context.Entry(existingQuiz).State = System.Data.Entity.EntityState.Deleted;
            return context.SaveChanges() > 0;
        }
        
    }
}