using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public class SubjectPictures
    {
        public int ID { get; set; }

        public int SubjectId { get; set; }

        public int PictureID { get; set; }
        public virtual Picture Picture { get; set; }
    }
}