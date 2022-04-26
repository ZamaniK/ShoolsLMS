using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public class AnswerChoice
    {
        [Key]
        [Required]
        public int AnswerChoiceId { get; set; }
        [Required]
        public string Choices { get; set; }
        [Required]
        public bool IsCorrect { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}