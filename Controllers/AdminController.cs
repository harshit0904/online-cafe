using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using WebApplicationExcoticMandi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data;
using PagedList;

namespace WebApplicationExcoticMandi.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Product()
        {
            ViewBag.Message = "Your Product Page";
            var products = db.Products.Where(p => p.IsActive == true);
            return View(products);

        }
        
        public ActionResult ProductList()
        {
            var product = db.Products.ToList();
            return View(product);
        }

        public ActionResult OrderList(int page = 1, int pageSize = 5)
        {
            var orders = db.Orders.OrderBy(c => c.OrderId);
            return View(orders.ToPagedList(page, pageSize));
        }

       public ActionResult CustomerList()
        {
            var customers = db.Customers.Include("ApplicationUser").ToList();
            return View(customers);
        }
       public ActionResult Index4(string sortOrder)
       {

           ViewBag.TotalSortParm = String.IsNullOrEmpty(sortOrder) ? "Total_desc" : "";
           ViewBag.OrderDateSortParm = sortOrder == "OrderDate" ? "OrderDate_desc" : "OrderDate";

           var orders = db.Orders.AsQueryable();


           switch (sortOrder)
           {
               case "Total_desc":
                   orders = orders.OrderByDescending(c => c.Total);
                   break;
               case "OrderDate":
                   orders = orders.OrderBy(c => c.OrderDate);
                   break;
               case "OrderDate_desc":
                   orders = orders.OrderByDescending(c => c.OrderDate);
                   break;

               default:
                   orders = orders.OrderBy(c => c.OrderDate);
                   break;

           }

           return View(orders);


       }
      
        public ActionResult SendAdminMail()
        {
            return View();
        }

        public ActionResult OrderDispatched(int? OrderId)
        {
            if (OrderId == null)
                return RedirectToAction("OrderList");

            var order = db.Orders.Include(a=>a.Customer).SingleOrDefault(a => a.OrderId == OrderId);


            string to = order.Customer.ApplicationUser.Email ;
            
            string subject = "order dispatched";
            string body = "<h3>Your Order with Order Id :"+ OrderId +" has been dispatched</h3>";

            string status = Utility.OrderMail(to, subject, body);

            ViewBag.status = status;
            return View();
        }

        public ActionResult CustomerDelete(string UserId)
        {
            var customer = db.Customers.SingleOrDefault(u => u.CustomerId == UserId);
            if (customer == null)
                HttpNotFound();

            return View(customer);
        }
        [HttpPost]
        [ActionName("CustomerDelete")]
        public ActionResult CustomerDeleteConfirm(string UserId)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            var customer = db.Customers.SingleOrDefault(a => a.CustomerId == UserId);
            if (customer != null)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    db.Customers.Remove(customer);
                    db.SaveChanges();

                    var user = UserManager.FindById(UserId);
                    var logins = user.Logins;
                    var rolesForUser = UserManager.GetRoles(UserId);

                    foreach (var login in logins.ToList())
                    {
                        UserManager.RemoveLogin(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                    }
                    if (rolesForUser.Count() > 0)
                    {
                        foreach (var item in rolesForUser.ToList())
                        {
                            var result = UserManager.RemoveFromRole(user.Id, item);
                        }
                    }
                    UserManager.Delete(user);
                    transaction.Commit();

                }
            }
            return RedirectToAction("CustomerList");
        }

       
    }
}