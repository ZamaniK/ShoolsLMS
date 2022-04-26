using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd - MM - yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PublishedDate { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        [Required]
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        public string ApplicationUserID { get; set; }
        public ApplicationUser User { get; set; }

        public virtual List<BooksBookPDF> BooksBookPDFs { get; set; }

    }
}