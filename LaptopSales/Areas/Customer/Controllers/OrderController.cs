using LaptopSales.Data;
using LaptopSales.Models;
using LaptopSales.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaptopSales.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrderController : Controller
    {
        private ApplicationDbContext _db;
        private UserManager<IdentityUser> _userManager;

        public OrderController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        //Get Checkout method
        public IActionResult Checkout()
        {
            return View();
        }
        //Post Checkout Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order anOrder)
        {
            List<Laptops> laptops = HttpContext.Session.Get<List<Laptops>>("laptops");
            var UserId = HttpContext.Session.GetString("UserId");
            if (laptops != null)
            {
                foreach(var laptop in laptops)
                {
                    OrderDetails orderdetails = new OrderDetails();
                    orderdetails.LaptopId = laptop.Id;
                    orderdetails.UserId = UserId;
                    anOrder.OrderDetails.Add(orderdetails);
                }
            }
            anOrder.OrderNo = GetOrderNo();
            _db.Order.Add(anOrder);
            await _db.SaveChangesAsync();
            HttpContext.Session.Set("laptops", new List<Laptops>());
            return View();
        }

        public string GetOrderNo()
        {
            int rowCount = _db.Order.ToList().Count() + 1;
            return rowCount.ToString("000");
        }

        public IActionResult Orders()
        {
            var UserId = HttpContext.Session.GetString("UserId");
            var orders = _db.OrderDetails.Include(l=>l.Order).Include(l=>l.Laptops).Include(l=>l.User).Where(l=>l.UserId == UserId).ToList();
            return View(orders);
        }
    }
}
