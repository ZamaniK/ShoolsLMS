using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.Data
{
    public class ADLog
    {
        [Required]
        public string FullName { get; set; }
        [Key]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string RetypePassword { get; set; }
    }
}