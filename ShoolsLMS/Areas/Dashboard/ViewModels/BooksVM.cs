using ShoolsLMS.Models;
using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Areas.Dashboard.ViewModels
{
    public class BookListingModel
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }

        public string SearchTerm { get; set; }
        public int? SubjectID { get; set; }
        public Pager Pager { get; set; }

    }

    public class BookActionModel
    {
        public int ID { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public string Title { get; set; }
        public string BookPDFIDs { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public string UploadedBy { get; set; }
        public virtual ApplicationUser User { get; set; }

        public DateTime? DatePublished { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
        public List<BooksBookPDF> BooksBookPDFs { get; set; }



    }
}