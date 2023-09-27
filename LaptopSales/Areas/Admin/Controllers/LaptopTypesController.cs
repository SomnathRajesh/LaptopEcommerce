using LaptopSales.Data;
using LaptopSales.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaptopSales.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class LaptopTypesController : Controller
    {
        private ApplicationDbContext _db;

        public LaptopTypesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.LaptopTypes.ToList());
        }

        //Get Create Method

        public ActionResult Create()
        {
            return View();
        }

        //Post Create method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LaptopTypes laptopTypes)
        {
            if(ModelState.IsValid)
            {
                _db.LaptopTypes.Add(laptopTypes);
                await _db.SaveChangesAsync();
                TempData["save"] = "Laptop Type has been saved";
                return RedirectToAction(nameof(Index));
                
            }
            return View(laptopTypes);
        }

        //Get Edit Method

        public ActionResult Edit(int? id)
        {
            if(id== null)
            {
                return NotFound();
            }
            var laptopType = _db.LaptopTypes.Find(id);
            if(laptopType == null)
            {
                return NotFound();
            }
            return View(laptopType);
        }

        //Post Edit method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LaptopTypes laptopTypes)
        {
            if (ModelState.IsValid)
            {
                _db.Update(laptopTypes);
                await _db.SaveChangesAsync();
                TempData["update"] = "Laptop Type has been updated";
                return RedirectToAction(nameof(Index));
            }
            return View(laptopTypes);
        }

        //Get Details Method

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var laptopType = _db.LaptopTypes.Find(id);
            if (laptopType == null)
            {
                return NotFound();
            }
            return View(laptopType);
        }

        //Post Details method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(LaptopTypes laptopTypes)
        {
            return RedirectToAction(nameof(Index));
        }

        //Get Delete Method

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var laptopType = _db.LaptopTypes.Find(id);
            if (laptopType == null)
            {
                return NotFound();
            }
            return View(laptopType);
        }

        //Post Delete method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id,LaptopTypes laptopTypes)
        {
            if(id == null)
            {
                return NotFound();
            }
            if(id!=laptopTypes.Id)
            {
                return NotFound();
            }
            var laptopType = _db.LaptopTypes.Find(id);
            if (laptopType == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Remove(laptopType);
                await _db.SaveChangesAsync();
                TempData["delete"] = "Laptop Type has been deleted";
                return RedirectToAction(nameof(Index));
            }
            return View(laptopTypes);
        }
    }
}
