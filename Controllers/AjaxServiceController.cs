using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationExcoticMandi.Models;

namespace WebApplicationExcoticMandi.Controllers
{
    public class AjaxServiceController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public JsonResult GetCategory()
        {

            var categories = db.Categories.Select(a => new { a.CategoryId, a.CategoryName });

            //return Json(states, JsonRequestBehavior.AllowGet);
            return Json(new { Categories = categories }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetProduct()
        {

            var products = db.Products.Select(a => new { a.ProductId, a.ProductName, a.Category.CategoryName });


            return Json(new { Products = products }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetProductByCategory(int CategoryId)
        {

            var products = db.Products.Where(a => a.CategoryId == CategoryId).Select(a => new { a.ProductId, a.ProductName, a.Category.CategoryName });


            return Json(new { Products = products }, JsonRequestBehavior.AllowGet);

        }
    }
}