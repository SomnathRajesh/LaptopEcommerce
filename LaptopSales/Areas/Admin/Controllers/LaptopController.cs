using LaptopSales.Data;
using LaptopSales.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LaptopSales.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class LaptopController : Controller
    {
        private ApplicationDbContext _db;
        private IWebHostEnvironment _environment;

        public LaptopController(ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }
        public IActionResult Index()
        {
            return View(_db.Laptops.Include(l=>l.LaptopTypes).Include(t=>t.Tags).ToList());
        }

        //Post Index Method 
        [HttpPost]
        public IActionResult Index(decimal? lowAmount, decimal? hugeAmount)
        {
            var laptops = _db.Laptops.Include(c=>c.LaptopTypes).Include(c=>c.Tags).Where(c=>c.Price>=lowAmount&&c.Price<=hugeAmount).ToList();
            if(lowAmount==null || hugeAmount == null)
            {
                laptops = _db.Laptops.Include(c => c.LaptopTypes).Include(c => c.Tags).ToList();
            }
            return View(laptops);
        }

        //Get create method

        public IActionResult Create()
        {
            ViewData["laptopTypeId"] = new SelectList(_db.LaptopTypes.ToList(),"Id","LaptopType");
            ViewData["tagId"] = new SelectList(_db.Tags.ToList(), "Id", "TagName");
            return View();
        }

        //Post create method

        [HttpPost]
        public async Task<IActionResult> Create(Laptops laptop,IFormFile? image)
        {
            //var imageErrors = ModelState["Image"].Errors;

            if (ModelState.IsValid)
            {
                var searchLaptop = _db.Laptops.FirstOrDefault(c=>c.Name== laptop.Name);
                if (searchLaptop != null)
                {
                    ViewBag.message = "This product already exists";
                    ViewData["laptopTypeId"] = new SelectList(_db.LaptopTypes.ToList(), "Id", "LaptopType");
                    ViewData["tagId"] = new SelectList(_db.Tags.ToList(), "Id", "TagName");
                    return View(laptop);
                }
                if(image!=null)
                {
                    var name=Path.Combine(_environment.WebRootPath+"/Images",Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    laptop.Image = "Images/"+image.FileName;
                }
                if (image == null)
                {
                    laptop.Image = "Images/Noimage.jpg";
                }
                _db.Laptops.Add(laptop);
                await _db.SaveChangesAsync();
                TempData["save"] = "Laptop has been saved";
                return RedirectToAction(nameof(Index));
            }
            return View(laptop);   
        }

        //Get Edit method
        public ActionResult Edit(int? id)
        {
            ViewData["laptopTypeId"] = new SelectList(_db.LaptopTypes.ToList(), "Id", "LaptopType");
            ViewData["tagId"] = new SelectList(_db.Tags.ToList(), "Id", "TagName");
            if(id==null)
            {
                return NotFound();
            }
            var laptop = _db.Laptops.Include(c=>c.LaptopTypes).Include(c=>c.Tags).FirstOrDefault(c=>c.Id==id);
            if(laptop == null)
            {
                return NotFound();
            }
            return View(laptop);
        }

        //POST Edit Method
        [HttpPost]
        public async Task<IActionResult> Edit(Laptops laptop, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_environment.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    laptop.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    laptop.Image = "Images/Noimage.jpg";
                }
                _db.Laptops.Update(laptop);
                await _db.SaveChangesAsync();
                TempData["update"] = "Laptop Details has been updated";
                return RedirectToAction(nameof(Index));
            }
            return View(laptop);
        }

        //GET Details Method
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var product = _db.Laptops.Include(c=>c.LaptopTypes).Include(c=>c.Tags).FirstOrDefault(c=>c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //Get Delete Method

        public ActionResult Delete(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var laptop = _db.Laptops.Include(c => c.LaptopTypes).Include(c => c.Tags).Where(c => c.Id == id).FirstOrDefault();
            if(laptop == null)
            {
                return NotFound();
            }
            return View(laptop);
        }

        //Post Delete Method
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var laptop = _db.Laptops.FirstOrDefault(c=>c.Id == id);
            if(laptop == null)
            {
                return NotFound();
            }
            _db.Laptops.Remove(laptop);
            await _db.SaveChangesAsync();
            TempData["delete"] = "Laptop has been deleted";
            return RedirectToAction(nameof(Index));
        }
    }
}
