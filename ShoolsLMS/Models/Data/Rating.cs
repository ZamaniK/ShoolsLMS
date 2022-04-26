using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int stars { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}