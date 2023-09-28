using System.Diagnostics;
using LaptopSales.Data;
using LaptopSales.Models;
using LaptopSales.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace LaptopSales.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index(int? page)
        {
            return View(_db.Laptops.Include(c=>c.LaptopTypes).Include(c=>c.Tags).ToList().ToPagedList(page??1,6));
        }

        //Post Index Method for filter
        [HttpPost]
        public IActionResult Index(int? page, decimal? lowAmount, decimal? hugeAmount, string? searchInput)
        {
            var laptops = _db.Laptops.Include(c => c.LaptopTypes).Include(c => c.Tags).Where(c => c.Price >= lowAmount && c.Price <= hugeAmount).Where(c=> c.Name.Contains(searchInput) || c.LaptopTypes.LaptopType.Contains(searchInput) || c.Tags.TagName.Contains(searchInput)).ToList();
            if (searchInput!=null && (lowAmount == null || hugeAmount == null))
            {
                laptops = _db.Laptops.Include(c => c.LaptopTypes).Include(c => c.Tags).Where(c => c.Name.Contains(searchInput) || c.LaptopTypes.LaptopType.Contains(searchInput) || c.Tags.TagName.Contains(searchInput)).ToList();
            }
            else if(searchInput==null && lowAmount!=null && hugeAmount!=null )
            {
                laptops = _db.Laptops.Include(c => c.LaptopTypes).Include(c => c.Tags).Where(c => c.Price >= lowAmount && c.Price <= hugeAmount).ToList();
            }
            else if(searchInput==null && lowAmount==null || hugeAmount == null)
            {
                laptops = _db.Laptops.Include(c => c.LaptopTypes).Include(c => c.Tags).ToList();
            }
            return View(laptops.ToPagedList(page??1,6));
        }

        ////Post Index Method for search
        //[HttpPost]
        //public IActionResult SearchIndex(int? page, string? searchInput)
        //{
        //    var laptops = _db.Laptops.Include(c => c.LaptopTypes).Include(c => c.Tags).Where(c => c.Name.Contains(searchInput) || c.LaptopTypes.LaptopType.Contains(searchInput) || c.Tags.TagName.Contains(searchInput)).ToList();
        //    if (searchInput==null)
        //    {
        //        laptops = _db.Laptops.Include(c => c.LaptopTypes).Include(c => c.Tags).ToList();
        //    }
        //    return View(laptops.ToPagedList(page ?? 1, 6));
        //}


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //Get product details method

        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var laptop = _db.Laptops.Include(c=>c.LaptopTypes).FirstOrDefault(c=> c.Id == id);
            if(laptop == null)
            {
                return NotFound();
            }
            return View(laptop);
        }

        //Post product details method
        [HttpPost]
        [ActionName("Details")]
		public ActionResult LaptopDetails(int? id)
		{
            List<Laptops> laptops = new List<Laptops>();
			if (id == null)
			{
				return NotFound();
			}
			var laptop = _db.Laptops.Include(c => c.LaptopTypes).FirstOrDefault(c => c.Id == id);
			if (laptop == null)
			{
				return NotFound();
			}
            laptops = HttpContext.Session.Get<List<Laptops>>("laptops");
            if(laptops== null)
            {
                laptops= new List<Laptops>();
            }
            laptops.Add(laptop);
            HttpContext.Session.Set("laptops", laptops);
			return View(laptop);
		}

        //Get Remove Method
        [ActionName("Remove")]
        public IActionResult RemoveFromCart(int? id)
        {
            List<Laptops> laptops = HttpContext.Session.Get<List<Laptops>>("laptops");
            if (laptops != null)
            {
                var laptop = laptops.FirstOrDefault(c => c.Id == id);
                if (laptop != null)
                {
                    laptops.Remove(laptop);
                    HttpContext.Session.Set("laptops", laptops);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //Post Remove Method

        [HttpPost]
        public IActionResult Remove(int? id)
        {
            List<Laptops> laptops = HttpContext.Session.Get<List<Laptops>>("laptops");
            if (laptops!= null)
            {
                var laptop = laptops.FirstOrDefault(c => c.Id == id);
                if(laptop != null)
                {
                    laptops.Remove(laptop);
                    HttpContext.Session.Set("laptops", laptops);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //Get Product Cart method
        public IActionResult Cart()
        {
            List<Laptops> laptops = HttpContext.Session.Get<List<Laptops>>("laptops");
            if (laptops== null)
            {
                laptops = new List<Laptops>();
            }
            return View(laptops);
        }
	}
}