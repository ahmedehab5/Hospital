using Hospital.Models;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Controllers
{
    public class AdminController : Controller
    {

        private readonly UserManager<Person> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<Person> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var admins = _userManager.Users.OfType<Admin>().ToList();

            //// Map IdentityRole to RoleViewModel
            var adminViewModels = admins.Select(a => new AdminViewModel
            {
                Id = a.Id,
                Email = a.Email,                


            }).ToList();

            // Pass the RoleViewModel list to the view
            return View(adminViewModels);
            
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(AdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var adminUser = new Admin
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    EmailConfirmed = true,
                    FirstName=model.FName,
                    LastName=model.FName,
                    Age=model.Age,
                    Gender=model.Gender,
                    PhoneNumber=model.PhoneNumber,
                };

                var result = await _userManager.CreateAsync(adminUser, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        public async Task<IActionResult> Details(string? id)
        {
            
                if (id == null)
                    return NotFound();

                var admin = await _userManager.FindByIdAsync(id);

                if (admin == null)
                    return NotFound();

                return View(admin);
            
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
                return NotFound();
            var admin = await _userManager.FindByIdAsync(id);

            if (admin == null)
                return NotFound();
            return View(admin);

        }


        [HttpPost]
        public async Task<IActionResult> Edit(Admin admin)
        {


            try
            {
                var existingPatient = await _userManager.FindByIdAsync(admin.Id);
                if (existingPatient == null)
                {
                    return NotFound(); // If patient doesn't exist
                }

                existingPatient.FirstName = admin.FirstName;
                existingPatient.LastName = admin.LastName;
                existingPatient.Email = admin.Email;
                existingPatient.Age = admin.Age;
                existingPatient.PhoneNumber = admin.PhoneNumber;

                var result = await _userManager.UpdateAsync(existingPatient);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(admin);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
                return NotFound();

            var admin = await _userManager.FindByIdAsync(id);

            if (admin == null)
                return NotFound();

            return View(admin);
        }



        [HttpPost]
        public async Task<IActionResult> Delete(Admin admin)
        {
            try
            {
                var ToBeDeleted = await _userManager.FindByIdAsync(admin.Id);
                if (ToBeDeleted == null)
                {
                    return NotFound(); // If patient doesn't exist
                }

                await _userManager.DeleteAsync(ToBeDeleted);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(admin);
            }
        }


    }
}
