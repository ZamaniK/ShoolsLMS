using Microsoft.Graph;
using ShoolsLMS.Models;
using ShoolsLMS.Services;
using ShoolsLMS.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoolsLMS.Controllers
{
    public class SubjectBooksController : Controller
    {
        // GET: SubjectBooks
        public ActionResult Index()
        {

            SubjectBookVM model = new SubjectBookVM();
            BookService bookService = new BookService();
            SubjectsService subjectService = new SubjectsService();

            model.Books = bookService.GetAllBooks();
            model.Subjects = subjectService.GetAllSubjects();
            return View(model);
        }

         /*public ActionResult GetAttachment(long id)
         {
             FileAttachment attachment;

             using (var db = new ApplicationDbContext())
             {
                 attachment = db.BookPDFs.FirstOrDefault(x => x.ID == id);
             }

             return File(attachment.Id, "application/force-download", Path.GetFileName(attachment.Name));
         }
         */
        
        [HttpGet]
        public ActionResult GetPDF(string fileName)
        {

            string filePath = "~/PDFs/site/" + fileName;
            Response.Headers.Add("Content-Disposition", "inline; filename=" + fileName);
            return File(filePath, "application/pdf");
        }


        //PDF Action

        public FileResult OpenPDF()
        {
            return File(Server.MapPath("/PDFs/site/ Design Class Diagram.pdf"), "application/pdf");
        }

        public FileResult DownloadPDF()
        {
            string filepath = Server.MapPath("/PDFs/site/Design.pdf");
            byte[] pdfByte = GetBytesFromFile(filepath);
            return File(pdfByte, "application/pdf", "Design.pdf");
        }

        public FileResult DisplayPDF()
        {
            string filepath = Server.MapPath("/PDFs/site/Design.pdf");
            byte[] pdfByte = GetBytesFromFile(filepath);
            return File(pdfByte, "application/pdf");
        }
        public PartialViewResult PDFPartialView()
        {
            return PartialView();
        }

   

        public byte[] GetBytesFromFile(string filepath)
        {

            //this method is limited to 2^32 byte files (4.2 GB)
            FileStream fs = null;
            try
            {
                fs = System.IO.File.OpenRead(filepath);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                return bytes;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }
    }
}