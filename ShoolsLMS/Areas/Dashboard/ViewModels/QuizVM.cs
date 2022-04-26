using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Areas.Dashboard.ViewModels
{
    public class QuizListingModel
    {
        public IEnumerable<Quiz> Quizs { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }

        public string SearchTerm { get; set; }
        public int? SubjectID { get; set; }
        public Pager Pager { get; set; }

    }

    public class QuizActionModel
    {
        public int ID { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public string QuizName { get; set; }
        public DateTime? StartTime { get; set; }
        
        public int Score { get; set; }

        public IEnumerable<Subject> Subjects { get; set; }

    }
}