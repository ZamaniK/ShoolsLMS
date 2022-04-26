using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using ShoolsLMS.Areas.Dashboard.ViewModels;
using ShoolsLMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShoolsLMS.Areas.Dashboard.Controllers
{
    public class RolesController : Controller
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


        public RolesController()
        {
        }

        public RolesController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public ActionResult Index(string searchTerm, int? page)
        {
            RolesListingModel model = new RolesListingModel();

            int recordSize = 10;
            page = page ?? 1;


            model.SearchTerm = searchTerm;

            model.Roles = SearchRoles(searchTerm, page.Value, recordSize);
            //model.Users = accomodationPackageService.GetAllAccomodationPackages();

            var totalRecords = SearchRolesCount(searchTerm);
            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);
        }


        public IEnumerable<IdentityRole> SearchRoles(string searchTerm, int page, int recordSize)
        {
            var roles = RoleManager.Roles.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                roles = roles.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            var skip = (page - 1) * recordSize;
            return roles.OrderBy(x => x.Name).Skip(skip).Take(recordSize).ToList();
        }


        public int SearchRolesCount(string searchTerm)
        {

            var roles = RoleManager.Roles.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                roles = roles.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            return roles.Count();
        }

        [HttpGet]
        public async Task<ActionResult> Action(string ID)
        {
            RolesActionModel model = new RolesActionModel();

            if (!string.IsNullOrEmpty(ID))  //we are trying to edit a record
            {
                var role = await RoleManager.FindByIdAsync(ID);

                model.ID = role.Id;
                model.Name = role.Name;
            }
            return PartialView("_Action", model);
        }


        [HttpPost]
        public async Task<JsonResult> Action(RolesActionModel model)
        {
            JsonResult json = new JsonResult();

            IdentityResult result = null;

            if (!string.IsNullOrEmpty(model.ID))
            {
                var role = await RoleManager.FindByIdAsync(model.ID);

                role.Name = model.Name;
                result = await RoleManager.UpdateAsync(role);
            }
            else
            {

                var role = new IdentityRole
                {
                    Name = model.Name
                };
                result = await RoleManager.CreateAsync(role);
            }
            json.Data = new { Success = result.Succeeded, Message = string.Join(", ", result.Errors) };
            return json;
        }


        [HttpGet]
        public async Task<ActionResult> Delete(string ID)
        {
            RolesActionModel model = new RolesActionModel();
            var role = await RoleManager.FindByIdAsync(ID);

            model.ID = role.Id;
            return PartialView("_Delete", model);
        }


        [HttpPost]
        public async Task<JsonResult> Delete(RolesActionModel model)
        {
            JsonResult json = new JsonResult();

            IdentityResult result = null;
            if (!string.IsNullOrEmpty(model.ID))
            {
                var role = await RoleManager.FindByIdAsync(model.ID);

                result = await RoleManager.DeleteAsync(role);
                json.Data = new { Success = result.Succeeded, Message = string.Join(", ", result.Errors) };
            }
            else
            {
                json.Data = new { Success = false, Message = "Invalid role." };
            }
            return json;
        }
    }
}