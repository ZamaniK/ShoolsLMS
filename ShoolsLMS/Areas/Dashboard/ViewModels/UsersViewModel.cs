using Microsoft.AspNet.Identity.EntityFramework;
using ShoolsLMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Areas.Dashboard.ViewModels
{
    public class UsersListingModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public string SearchTerm { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }
        public string RoleID { get; set; }

        public Pager Pager { get; set; }
    }

    public class UsersActionModel
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Username { get; set; }

        public string Gender { get; set; }
        [Display(Name = "Home Address")]
        [DataType(DataType.MultilineText)]
        public string HomeAddress { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class UserRolesModel
    {
        public string UserID { get; set; }

        public IEnumerable<IdentityRole> UserRoles { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}