using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationExcoticMandi.Models;
using Microsoft.AspNet.Identity;

namespace WebApplicationExcoticMandi.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        ApplicationDbContext storeDB = new ApplicationDbContext();
        public ActionResult AddressAndPayment()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewBag.total = cart.GetTotal();
            return View();
        }
        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);

            try
            {
                var cart = ShoppingCart.GetCart(this.HttpContext);
               
                    order.Username = User.Identity.Name;
                    order.CustomerId = User.Identity.GetUserId();
                    order.OrderDate = DateTime.Now;
                    order.Total = cart.GetTotal();

                    //Save Order
                    storeDB.Orders.Add(order);
                    storeDB.SaveChanges();
                    //Process the order
                   
                    cart.CreateOrder(order);

                    return RedirectToAction("Complete",new { id = order.OrderId });
                }
            
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }
        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = storeDB.Orders.Any(o => o.OrderId == id && o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }

       
    }
}