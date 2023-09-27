using LaptopSales.Data;
using LaptopSales.Models;
using LaptopSales.Utility;
using Microsoft.AspNetCore.Mvc;

namespace LaptopSales.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
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
            if (laptops != null)
            {
                foreach(var laptop in laptops)
                {
                    OrderDetails orderdetails = new OrderDetails();
                    orderdetails.LaptopId = laptop.Id;
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
    }
}
