using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public class TeacherPictures
    {
        public int ID { get; set; }

        public int TeacherId { get; set; }

        public int PictureID { get; set; }
        public virtual Picture Picture { get; set; }
    }
}