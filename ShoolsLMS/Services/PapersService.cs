using ShoolsLMS.Models;
using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Services
{
    public class PapersService
    {  
          public IEnumerable<Paper> GetAllPapers()
            {
                var context = new ApplicationDbContext();
                return context.Papers.ToList();
            }

            public IEnumerable<Paper> SearchPapers(string searchTerm, int? PaperID, int page, int recordSize)
            {
                var context = new ApplicationDbContext();
                var papers = context.Papers.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    papers = papers.Where(a => a.Title.ToLower().Contains(searchTerm.ToLower()));
                }
                if (PaperID.HasValue && PaperID.Value > 0)
                {
                    papers = papers.Where(a => a.PaperId == PaperID.Value);
                }

                var skip = (page - 1) * recordSize;

                return papers.OrderBy(t => t.PaperId == PaperID.Value).Skip(skip).Take(recordSize).ToList();
            }

            public Paper GetPaperByID(int ID)
            {
                var context = new ApplicationDbContext();

                return context.Papers.Find(ID);
            }
            public IEnumerable<Paper> GetAllPapersBySubject(int subjectID)
            {
                var context = new ApplicationDbContext();
                return context.Papers.Where(x => x.SubjectId == subjectID).ToList();
            }


            public bool SavePaper(Paper paper)
            {
                var context = new ApplicationDbContext();

                context.Papers.Add(paper);
                return context.SaveChanges() > 0;
            }

            public bool UpdatePaper(Paper paper)
            {
                var context = new ApplicationDbContext();

                var existingPaper = context.Papers.Find(paper.PaperId);
                context.Entry(existingPaper).State = System.Data.Entity.EntityState.Modified;
                return context.SaveChanges() > 0;
            }

            public bool DeletePaper(Paper paper)
            {
                var context = new ApplicationDbContext();

                var existingPaper = context.Papers.Find(paper.PaperId);
                context.Entry(existingPaper).State = System.Data.Entity.EntityState.Deleted;
                return context.SaveChanges() > 0;
            }

        
    }
}