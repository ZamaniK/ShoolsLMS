using ShoolsLMS.Models;
using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Services
{
    public class BookService
    {
        public IEnumerable<Book> GetAllBooks()
        {
            var context = new ApplicationDbContext();
            return context.Books.ToList();
        }

        public IEnumerable<Book> SearchBooks(string searchTerm, int? BookID, int page, int recordSize)
        {
            var context = new ApplicationDbContext();
            var books = context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                books = books.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            if (BookID.HasValue && BookID.Value > 0)
            {
                books = books.Where(a => a.BookId == BookID.Value);
            }

            var skip = (page - 1) * recordSize;

            return books.OrderBy(t => t.BookId == BookID.Value).Skip(skip).Take(recordSize).ToList();
        }

        public Book GetBookByID(int ID)
        {
            var context = new ApplicationDbContext();

            return context.Books.Find(ID);
        }
        public IEnumerable<Book> GetAllBooksBySubject(int subjectID)
        {
            var context = new ApplicationDbContext();
            return context.Books.Where(x => x.SubjectId == subjectID).ToList();
        }


        public bool SaveBook(Book book)
        {
            var context = new ApplicationDbContext();

            context.Books.Add(book);
            return context.SaveChanges() > 0;
        }

        public bool UpdateBook(Book book)
        {
            var context = new ApplicationDbContext();

            var existingBook = context.Books.Find(book.BookId);
            context.BooksBookPDFs.RemoveRange(existingBook.BooksBookPDFs);
            context.Entry(existingBook).State = System.Data.Entity.EntityState.Modified;
            context.BooksBookPDFs.AddRange(book.BooksBookPDFs);
            return context.SaveChanges() > 0;
        }

        public bool DeleteGrade(Book book)
        {
            var context = new ApplicationDbContext();

            var existingBook = context.Books.Find(book.BookId);
            context.BooksBookPDFs.RemoveRange(existingBook.BooksBookPDFs);
            context.Entry(existingBook).State = System.Data.Entity.EntityState.Deleted;
            context.BooksBookPDFs.AddRange(book.BooksBookPDFs);
            return context.SaveChanges() > 0;
        }
        public List<BooksBookPDF> GetBookPDFByBookID(int bookID)
        {
            var context = new ApplicationDbContext();
            return context.Books.Find(bookID).BooksBookPDFs.ToList();
        }
    }
}