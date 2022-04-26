using ShoolsLMS.Models;
using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Services
{
    public class AssignmentServive
    {
        public IEnumerable<Assignment> GetAllAssignments()
        {
            var context = new ApplicationDbContext();
            return context.Assignment.ToList();
        }

        public IEnumerable<Assignment> SearchAssignment(string searchTerm, int? assignID, int page, int recordSize)
        {
            var context = new ApplicationDbContext();
            var assignment = context.Assignment.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                assignment = assignment.Where(a => a.AssignmentName.ToLower().Contains(searchTerm.ToLower()));
            }

            if (assignID.HasValue && assignID.Value > 0)
            {
                assignment = assignment.Where(a => a.AssignmentId == assignID.Value);
            }

            var skip = (page - 1) * recordSize;

            return assignment.OrderBy(t => t.AssignmentId == assignID.Value).Skip(skip).Take(recordSize).ToList();
        }
    }
}