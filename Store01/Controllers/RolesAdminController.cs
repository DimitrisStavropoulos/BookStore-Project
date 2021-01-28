using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Net; //http , async
using System.Threading.Tasks;//http, async
using Store01.Models;
using Store01.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace Store01.Controllers
{
    //Role management (Admin) Controller


    [Authorize(Roles = "Admin")]
    public class RolesAdminController : Controller
    {
        //Obtain Rolemanager and UserManager instances for use in the controller
        public RolesAdminController()
        {
        }

        public RolesAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
             HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }










        // GET: RolesAdmin
        public ActionResult Index()
        {
            //Return a list of all Roles in the system.
            return View(RoleManager.Roles);
        }

        // GET: RolesAdmin/Details/5
        //details becomes asynchronous, program can continue with other actions while
        //this task is running.
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //(async) find role by id
            var role = await RoleManager.FindByIdAsync(id);
            // Initialize list of Users in this Role
            var users = new List<ApplicationUser>();

            // Get the list of Users in this Role
            foreach (var user in UserManager.Users.ToList())
            {       //determine if user is in a role based on Id,Name
                if (await UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    users.Add(user);
                }
            }

            ViewBag.Users = users;
            //for message in view if no users are found
            ViewBag.UserCount = users.Count();
            return View(role);
        }

        // GET: RolesAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolesAdmin/Create
        //attempts to create a new role based on IdentityRole object provided as an
        //input parameter.Asynchronous method that takes a RoleViewModel as input
        [HttpPost]
        public async Task<ActionResult> Create(RoleViewModel roleViewModel)
        {
            //check if RoleViewModel state valid 
            if (ModelState.IsValid)
            {
                //Create  IdentityRole type variable
                var role = new IdentityRole(roleViewModel.Name);
                
                //Attempt (async) to create this role
                var roleresult = await RoleManager.CreateAsync(role);
                //if not success redirect to create with error message
                if (!roleresult.Succeeded)
                {
                    ModelState.AddModelError("", roleresult.Errors.First());
                    return View();
                }
                //if success redirect to index
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: RolesAdmin/Edit/5
        
        public async Task<ActionResult> Edit(string id)
        {//changed to string so that RoleManager.FindByIdAsyn can use it
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            RoleViewModel roleModel = new RoleViewModel { Id = role.Id, Name = role.Name };
            return View(roleModel);
        }

        // POST: RolesAdmin/Edit/5
        //similar logic
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleViewModel roleModel)
        {
            if (ModelState.IsValid)
            {
                var role = await RoleManager.FindByIdAsync(roleModel.Id);
                role.Name = roleModel.Name;
                await RoleManager.UpdateAsync(role);
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: RolesAdmin/Delete/5
        //similar to Edit but since we delete the view will be based on IdentityRole
        //rather than RoleViewModel
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: RolesAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //change method name avoid duplicate method
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var role = await RoleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return HttpNotFound();
                }
                IdentityResult result = await RoleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
