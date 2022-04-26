using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public class Lesson
    {
        public Lesson()
        {
        }

        public Lesson(int lessonId, string lessonName, int subjectId, Subject subject, string fileName, string applicationUserID, ApplicationUser user)
        {
            LessonId = lessonId;
            LessonName = lessonName;
            SubjectId = subjectId;
            Subject = subject;
            FileName = fileName;
            ApplicationUserID = applicationUserID;
            User = user;
        }

        [Key]
        public int LessonId { get; set; }

        [Required]
        public string LessonName { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }
        public string ApplicationUserID { get; set; }
        public ApplicationUser User { get; set; }
    }
}