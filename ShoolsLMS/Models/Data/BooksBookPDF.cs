using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public class BooksBookPDF
    {
        public int ID { get; set; }

        public int BookId { get; set; }

        public int BookPDFID { get; set; }
        public virtual BookPDF BookPDF { get; set; }
    }
}