using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationExcoticMandi.Models;
using PagedList;

namespace WebApplicationExcoticMandi.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Menu()
        {
            ViewBag.Message = "Your Menu Page";
            return View();
        }
        public ActionResult Category()
        {
            return View();
        }
        public ActionResult Products(int page = 1, int pageSize = 5)
        {
            ViewBag.Message = "Your Product Page";
            
            var products = db.Products.Where(p => p.IsActive == true);
            
            
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
             products = db.Products.OrderBy(c => c.ProductId);
            return View(products.ToPagedList(page, pageSize));
        }
        public ActionResult CascadeDropdown()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.ProductId = new SelectList(Enumerable.Empty<SelectList>(), "ProductId", "ProductName");
            return View();
        }
        public ActionResult ProductDetails(int? ProductId)
        {
            if (ProductId == null)
                return RedirectToAction("Products");

            var product = db.Products.SingleOrDefault(p => p.ProductId == ProductId && p.IsActive == true);
            return View(product);
        }
          
        public ActionResult Login()
        {
            ViewBag.Message = "Your Product Page";
            return View();
        }

        public ActionResult Search()
        {
            ViewBag.Message = "Tour Search Page";
            return View();
        }
        public ActionResult Offers()
        {
            ViewBag.Message = "Your Offer Page";
            return View();
        }
        public ActionResult Help()
        {
            ViewBag.Message = "Your Offer Page";
            return View();
        }
        public ActionResult PrivacyPolicy()
        {
            ViewBag.Message = " Privacy Policy Page";
            return View();
        }
        public ActionResult Signup()
        {
            ViewBag.Message = "Your Signup Page";
            return View();
        }
        public ActionResult Faq()
        {
            ViewBag.Message = "YOur FAQ Page";
            return View();
        }
        public ActionResult Terms()
        {
            ViewBag.Message = "Your Terms Page ";
            return View();
        }
        /*
        public ActionResult SendMail()
        {
            return View();
        }
         */
       
       
    }
}