using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public class StudentQuiz
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int StudentQuizId { get; set; }
        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        public double Marks { get; set; }
    }
}