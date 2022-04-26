using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public class Quiz
    {
        [Key]
        [Required]
        public int QuizId { get; set; }
        [Required]
        public string QuizName { get; set; }
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? StartTime { get; set; }
        
        [Required]
        public int Score { get; set; }

        public ICollection<Question> Question { get; set; }
        public ICollection<StudentQuiz> Students { get; set; }
    }
}