using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ShoolsLMS.Areas.Dashboard.ViewModels;
using ShoolsLMS.Models;
using ShoolsLMS.Models.Data;
using ShoolsLMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShoolsLMS.Areas.Dashboard.Controllers
{
    public class UsersController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        // GET: Dashboard/Users
        public async Task<ActionResult> Index(string searchTerm, string roleID, int? page)
        {
            UsersListingModel model = new UsersListingModel();

            int recordSize = 5;
            page = page ?? 1;


            model.SearchTerm = searchTerm;
            model.RoleID = roleID;

            model.Users = await SearchUsers(searchTerm, roleID, page.Value, recordSize);
            model.Roles = RoleManager.Roles.ToList();

            var totalRecords = await SearchUsersCount(searchTerm, roleID);
            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);
        }


        public async Task<IEnumerable<ApplicationUser>> SearchUsers(string searchTerm, string roleID, int page, int recordSize)
        {
            var users = UserManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(a => a.Email.ToLower().Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrEmpty(roleID))
            {
                var role = await RoleManager.FindByIdAsync(roleID);
                var userIDs = role.Users.Select(x => x.UserId).ToList();

                users = users.Where(x => userIDs.Contains(x.Id));
            }


            var skip = (page - 1) * recordSize;

            return users.OrderBy(x => x.Email).Skip(skip).Take(recordSize).ToList();
        }


        public async Task<int> SearchUsersCount(string searchTerm, string roleID)
        {
            var users = UserManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(a => a.Email.ToLower().Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrEmpty(roleID))
            {
                var role = await RoleManager.FindByIdAsync(roleID);
                var userIDs = role.Users.Select(x => x.UserId).ToList();

                users = users.Where(x => userIDs.Contains(x.Id));
            }

            return users.Count();
        }

        [HttpGet]
        public async Task<ActionResult> Action(string ID)
        {
            UsersActionModel model = new UsersActionModel();

            if (!string.IsNullOrEmpty(ID))  //we are trying to edit a record
            {
                var user = await UserManager.FindByIdAsync(ID);

                model.ID = user.Id;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;

                model.Email = user.Email;
                model.Gender = user.Gender;
                model.Gender = user.Gender;
                model.HomeAddress = user.HomeAddress;
            }
            return PartialView("_Action", model);
        }


        [HttpPost]
        public async Task<JsonResult> Action(UsersActionModel model)
        {
            JsonResult json = new JsonResult();

            IdentityResult result = null;

            if (!string.IsNullOrEmpty(model.ID))
            {
                var user = await UserManager.FindByIdAsync(model.ID);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.Username;

                user.Email = model.Email;
                user.Gender = model.Gender;
                user.HomeAddress = model.HomeAddress;

                result = await UserManager.UpdateAsync(user);
            }
            else
            {
                Grade grade = new Grade();

                var user = new ApplicationUser();

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.Username;


                user.Email = model.Email;
                user.Gender = model.Gender;
                user.HomeAddress = model.HomeAddress;
                
                result = await UserManager.CreateAsync(user, model.Password);
            }
            json.Data = new { Success = result.Succeeded, Message = string.Join(", ", result.Errors) };
            return json;
        }


        [HttpGet]
        public async Task<ActionResult> Delete(string ID)
        {
            UsersActionModel model = new UsersActionModel();
            var user = await UserManager.FindByIdAsync(ID);

            model.ID = user.Id;
            return PartialView("_Delete", model);
        }


        [HttpPost]
        public async Task<JsonResult> Delete(UsersActionModel model)
        {
            JsonResult json = new JsonResult();

            IdentityResult result = null;
            if (!string.IsNullOrEmpty(model.ID))
            {
                var user = await UserManager.FindByIdAsync(model.ID);

                result = await UserManager.DeleteAsync(user);
                json.Data = new { Success = result.Succeeded, Message = string.Join(", ", result.Errors) };
            }
            else
            {
                json.Data = new { Success = false, Message = "Invalid user." };
            }
            return json;
        }

        [HttpGet]
        public async Task<ActionResult> UserRoles(string ID)
        {
            UserRolesModel model = new UserRolesModel();

            model.UserID = ID;
            var user = await UserManager.FindByIdAsync(ID);
            var userRolesIDs = user.Roles.Select(x => x.RoleId).ToList();

            model.UserRoles = RoleManager.Roles.Where(x => userRolesIDs.Contains(x.Id)).ToList();
            model.Roles = RoleManager.Roles.Where(x => !userRolesIDs.Contains(x.Id)).ToList();

            return PartialView("_UserRoles", model);
        }

        [HttpPost]
        public async Task<JsonResult> UserRoleOperation(string userID, string roleID, bool isDelete = false)
        {
            JsonResult json = new JsonResult();
            var user = await UserManager.FindByIdAsync(userID);
            var role = await RoleManager.FindByIdAsync(roleID);

            if (user != null && role != null)
            {
                IdentityResult result = null;
                if (!isDelete)
                {
                    result = await UserManager.AddToRoleAsync(userID, role.Name);
                }
                else
                {
                    result = await UserManager.RemoveFromRoleAsync(userID, role.Name);
                }
                json.Data = new { Success = result.Succeeded, Message = string.Join(", ", result.Errors) };
            }
            else
            {
                json.Data = new { Success = false, Message = "Invalid operation." };
            }
            return json;
        }

    }
}