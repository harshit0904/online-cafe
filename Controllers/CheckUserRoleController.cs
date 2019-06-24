using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationExcoticMandi.Models;

namespace WebApplicationExcoticMandi.Controllers
{
    [Authorize]
    public class CheckUserRoleController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: CheckUserRole
        public ActionResult Index()
        {

            if (IsAdminUser())
                return RedirectToAction("Index", "Admin");
            if (IsCustomerUser())
                return RedirectToAction("Index", "Customer");
            
            else throw new Exception("User Role Not Found");
        }

        public bool IsAdminUser()
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var UserRoles = UserManager.GetRoles(User.Identity.GetUserId());

            foreach (string role in UserRoles)
            {
                if (role == "Admin")
                    return true;
            }
            return false;
        }
        public bool IsCustomerUser()
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var UserRoles = UserManager.GetRoles(User.Identity.GetUserId());

            foreach (string role in UserRoles)
            {
                if (role == "Customer")
                    return true;
            }
            return false;
        }
    }
}