using ShoolsLMS.Models;
using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoolsLMS.DataRepos.Reposisoty
{
    public interface ISchoolLMSData
    {
        IRepository<ApplicationUser> Users { get; }
        IRepository<Lesson> Teachers { get; }
        IRepository<Grade> Grades { get; }
        IRepository<Subject> Subjects { get; }
        int SaveChanges();
    }
}
