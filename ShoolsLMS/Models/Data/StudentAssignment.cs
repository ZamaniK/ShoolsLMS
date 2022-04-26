using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public class StudentAssignment
    {
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        public int AssignmentId { get; set; }
        public virtual Assignment Assignment { get; set; }
    }
}