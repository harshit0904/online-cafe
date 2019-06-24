using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationExcoticMandi.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace WebApplicationExcoticMandi.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            if (TempData["Message"] != null)
                ViewBag.message = TempData["Message"];
            return View();
        }
        public ActionResult CustomerProfile()
        {
            string UserId = User.Identity.GetUserId();

            var customer = db.Customers.SingleOrDefault(a => a.CustomerId == UserId);
            return View(customer);
        }
    }
}