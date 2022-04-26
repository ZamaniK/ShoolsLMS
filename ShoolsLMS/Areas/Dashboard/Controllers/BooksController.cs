using Microsoft.AspNet.Identity;
using ShoolsLMS.Areas.Dashboard.ViewModels;
using ShoolsLMS.Infrastructure.FileHelpers;
using ShoolsLMS.Models.Data;
using ShoolsLMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoolsLMS.Areas.Dashboard.Controllers
{
    public class BooksController : Controller
    {
        // GET: Dashboard/Books
        BookService booksService = new BookService();
        SubjectsService subjectsService = new SubjectsService();
        DashboardService dashboardService = new DashboardService();

        public ActionResult Index(string searchTerm, int? subjectID, int? page)
        {
            BookListingModel model = new BookListingModel();

            int recordSize = 5;
            page = page ?? 1;

            model.SearchTerm = searchTerm;
            model.SubjectID = subjectID;

            model.Books = booksService.SearchBooks(searchTerm, subjectID, page.Value, recordSize);

            model.Subjects = subjectsService.GetAllSubjects();

            var totalRecords = subjectsService.SearchSubjectCount(searchTerm, subjectID);
            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);
        }

        [HttpGet]
        public ActionResult Action(int? ID)
        {
            BookActionModel model = new BookActionModel();

            if (ID.HasValue)  //we are trying to edit a record
            {
                var book = booksService.GetBookByID(ID.Value);
                string getuser = User.Identity.GetUserId();


                book.SubjectId = model.SubjectId;
                book.Name = model.Title;
                book.Description = model.Description;
                book.AuthorName = model.AuthorName;
                book.PublishedDate = model.DatePublished;
                book.User.Id = getuser;

                model.BooksBookPDFs = booksService.GetBookPDFByBookID(book.BookId);


            }
            model.Subjects = subjectsService.GetAllSubjects();
            return PartialView("_Action", model);
        }


        [HttpPost]
        public JsonResult Action(BookActionModel model)
        {
            JsonResult json = new JsonResult();

            var result = false;

            List<int> bookIDs = !string.IsNullOrEmpty(model.BookPDFIDs) ? model.BookPDFIDs.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>();
            var pdfs = dashboardService.GetPDFByID(bookIDs);

            if (model.ID > 0)
            {
                var book = booksService.GetBookByID(model.ID);
                string getuser = User.Identity.GetUserId();


                book.BookId = model.ID;
                book.SubjectId = model.SubjectId;
                book.Name = model.Title;
                book.Description = model.Description;
                book.AuthorName = model.AuthorName;
                book.ApplicationUserID = getuser;
                book.ApplicationUserID = model.UploadedBy;
                book.PublishedDate = model.DatePublished;
                book.BooksBookPDFs.Clear();
                book.BooksBookPDFs.AddRange(pdfs.Select(x => new BooksBookPDF() { BookId = book.BookId, BookPDFID = x.ID }));

                result = booksService.UpdateBook(book);
            }
            else
            {
                Book book = new Book();

                string getuser = User.Identity.GetUserId();

                book.SubjectId = model.SubjectId;
                book.Name = model.Title;
                book.Description = model.Description;
                book.AuthorName = model.AuthorName;
                book.PublishedDate = model.DatePublished;
                book.ApplicationUserID = getuser;
                book.ApplicationUserID = model.UploadedBy;
                book.BooksBookPDFs = new List<BooksBookPDF>();
                book.BooksBookPDFs.AddRange(pdfs.Select(x => new BooksBookPDF() { BookId = book.BookId, BookPDFID = x.ID }));

                result = booksService.SaveBook(book);


            }


            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on Books." };
            }
            return json;
        }


        [HttpGet]
        public ActionResult Delete(int ID)
        {
            BookActionModel model = new BookActionModel();

            var book = booksService.GetBookByID(ID);

            model.ID = book.BookId;


            return PartialView("_Delete", model);
        }


        [HttpPost]
        public JsonResult Delete(BookActionModel model)
        {
            JsonResult json = new JsonResult();

            var result = false;

            var book = booksService.GetBookByID(model.ID);



            result = booksService.DeleteGrade(book);

            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on Books." };
            }
            return json;
        }
    }
}