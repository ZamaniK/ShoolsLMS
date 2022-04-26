using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public class Certificate
    {
        [Key]
        [Required]
        public int CertificateId { get; set; }
        [Required]
        public string ApplicationUserID { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}