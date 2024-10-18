using Hospital.Models;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Controllers
{
    public class AdminController : Controller
    {

        private readonly UserManager<Person> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<Person> _signInManager;

		public AdminController(UserManager<Person> userManager, RoleManager<IdentityRole> roleManager, SignInManager<Person> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
			_signInManager = signInManager;
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
                    return RedirectToAction("Index","Admin");
                }

                foreach (var error in result.Errors)
                {
					Console.WriteLine(error.Description);
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


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }




        #region MyRegion
        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);

        //        if (user != null) // Check if user exists
        //        {
        //            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
        //            if (isPasswordValid) // Check if the password is correct
        //            {
        //                // Check if the user is an Admin
        //                var roles = await _userManager.GetRolesAsync(user);
        //                if (roles.Contains("Admin"))
        //                {
        //                    // Sign in the admin and redirect to the admin dashboard
        //                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
        //                    return RedirectToAction("Index", "Admin");
        //                }
        //                else
        //                {
        //                    // Display an error message if the user is not an Admin
        //                    ModelState.AddModelError(string.Empty, "Access Denied: You do not have permission to access this area.");
        //                    return View(model); // Return to the same view with the error message
        //                }
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Incorrect Email or Password");
        //            }
        //        }
        //        else // If this user doesn't exist
        //        {
        //            ModelState.AddModelError(string.Empty, "This Email doesn't exist.");
        //        }
        //    }

        //    return View(model);
        //} 
        #endregion


        #region MyRegion
        ////[Authorize(Roles = "Admin")]
        //[HttpPost]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);

        //        if (user != null) // Check if user exists
        //        {
        //            // First, check if the user is an Admin
        //            var roles = await _userManager.GetRolesAsync(user);
        //            if (roles.Contains("Admin"))
        //            {
        //                // Check if the password is correct
        //                bool isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
        //                if (isPasswordValid) // If password is valid, sign in
        //                {
        //                    await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
        //                    return RedirectToAction("Index", "Admin"); // Redirect to Admin dashboard
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("", "Incorrect Password.");
        //                }
        //            }
        //            else
        //            {
        //                // If the user is not an Admin, show an access denied message
        //                ModelState.AddModelError(string.Empty, "Access Denied: You do not have permission to access this area.");
        //            }
        //        }
        //        else // If the user doesn't exist
        //        {
        //            ModelState.AddModelError(string.Empty, "This Email doesn't exist.");
        //        }
        //    }

        //    return View(model); // Stay on the same page with the error messages
        //}
        #endregion






    }
}
