using ShoolsLMS.Models;
using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Services
{
    public class DashboardService
    {
        public bool SavePicture(Picture picture)
        {
            var context = new ApplicationDbContext();

            context.Pictures.Add(picture);
            return context.SaveChanges() > 0;
        }


        public IEnumerable<Picture> GetPicturesByID(List<int> pictureIDs)
        {
            var context = new ApplicationDbContext();
            return pictureIDs.Select(x => context.Pictures.Find(x)).ToList();
        }

        public bool SavePDF(BookPDF book)
        {
            var context = new ApplicationDbContext();

            context.BookPDFs.Add(book);
            return context.SaveChanges() > 0;
        }


        public IEnumerable<BookPDF> GetPDFByID(List<int> bookIDs)
        { 
            var context = new ApplicationDbContext();
            return bookIDs.Select(x => context.BookPDFs.Find(x)).ToList();
        }

    }
}