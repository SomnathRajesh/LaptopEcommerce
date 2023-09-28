using LaptopSales.Data;
using LaptopSales.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaptopSales.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperUser")]
    public class TagsController : Controller
    {
        private ApplicationDbContext _db;

        public TagsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.Tags.ToList());
        }

        //Get Create Method

        public ActionResult Create()
        {
            return View();
        }

        //Post Create method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tags tag)
        {
            if (ModelState.IsValid)
            {
                _db.Tags.Add(tag);
                await _db.SaveChangesAsync();
                TempData["save"] = "Tag has been saved";
                return RedirectToAction(nameof(Index));

            }
            return View(tag);
        }

        //Get Edit Method

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tag = _db.Tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        //Post Edit method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Tags tag)
        {
            if (ModelState.IsValid)
            {
                _db.Update(tag);
                await _db.SaveChangesAsync();
                TempData["update"] = "Tag has been updated";
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        //Get Details Method

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tag = _db.Tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        //Post Details method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(Tags tag)
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
            var tag = _db.Tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        //Post Delete method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, Tags tag)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != tag.Id)
            {
                return NotFound();
            }
            var t = _db.Tags.Find(id);
            if (t == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Remove(t);
                await _db.SaveChangesAsync();
                TempData["delete"] = "Tag has been deleted";
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }
    }
}
