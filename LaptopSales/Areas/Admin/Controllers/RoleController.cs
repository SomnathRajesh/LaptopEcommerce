using System.ComponentModel.DataAnnotations;
using LaptopSales.Areas.Admin.Models;
using LaptopSales.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaptopSales.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        ApplicationDbContext _db;
        UserManager<IdentityUser> _userManager;
        public RoleController(RoleManager<IdentityRole> roleManager,ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles;
            return View();
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            IdentityRole role = new IdentityRole();
            role.Name = name;
            var exists = await _roleManager.RoleExistsAsync(role.Name);
            if (exists)
            {
                ViewBag.msg = "The role exists already";
                ViewBag.name = name;
                return View();
            }
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                TempData["save"] = "Role has been added successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id; ViewBag.name = role.Name;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id,string name)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = name;
            var exists = await _roleManager.RoleExistsAsync(role.Name);
            if (exists)
            {
                ViewBag.msg = "The role exists already";
                ViewBag.name = name;
                return View();
            }
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                TempData["save"] = "Role has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id; ViewBag.name = role.Name;
            return View();
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            var result = await _roleManager.DeleteAsync(role);
            if(result.Succeeded) {
                TempData["delete"] = "Role has been deleted";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Assign()
        {
            ViewData["UserId"] = new SelectList(_db.ApplicationUsers.Where(c=>c.LockoutEnd<DateTime.Now || c.LockoutEnd==null).ToList(),"Id","UserName");
            ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(),"Name","Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Assign(UserRole userrole)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == userrole.UserId);
            var isCheckAssignedRole = await _userManager.IsInRoleAsync(user,userrole.RoleId);
            if (isCheckAssignedRole)
            {
                ViewBag.message = "This user has already been assigned this role";
                ViewData["UserId"] = new SelectList(_db.ApplicationUsers.Where(c => c.LockoutEnd < DateTime.Now || c.LockoutEnd == null).ToList(), "Id", "UserName");
                ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
                return View();
            }
            var role = await _userManager.AddToRoleAsync(user, userrole.RoleId);
            if(role.Succeeded)
            {
                TempData["save"] = "Role has been assigned to the user";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public ActionResult AssignedUserRole()
        {
            var result = from ur in _db.UserRoles
                         join r in _db.Roles on ur.RoleId equals r.Id
                         join a in _db.ApplicationUsers on ur.UserId equals a.Id
                         select new UserRoleMapping()
                         {
                             UserId = ur.UserId,
                             RoleId = ur.RoleId,
                             UserName = a.UserName,
                             RoleName = r.Name
                         };
            ViewBag.UserRoles = result;
            return View();
        }
    }
}
