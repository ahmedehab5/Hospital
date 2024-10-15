using Hospital.Helpers;
using Hospital.Models;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace Hospital.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<Person> _userManager;
		private readonly SignInManager<Person> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<Person> userManager, SignInManager<Person> signInManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
            //_roleManager = roleManager;
        }


        //[HttpGet]
        //public async Task<IActionResult> Details(string? id)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new ProfileViewModel
        //    {
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        Email = user.Email,
        //        PhoneNumber = user.PhoneNumber,
        //        Image = user.Image 
        //    };

        //    return View(model);
        //}

        //[Authorize]



        //[HttpGet]
        //public async Task<IActionResult> Details(string? id)
        //{
        //    if (id == null)
        //        return NotFound();

        //    var user = await _userManager.FindByIdAsync(id);

        //    if (user == null)
        //        return NotFound();

        //    return View(user);
        //}


        //[Authorize]  // Ensure only logged-in users can access this
        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
                id = _userManager.GetUserId(User);  // Get the current user's ID if not provided

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            return View(user);  // Return the user's profile view
        }


        //[HttpGet]
        //public async Task<IActionResult> Edit(string? id)
        //{
        //    if (id == null)
        //        return NotFound();
        //    var user = await _userManager.FindByIdAsync(id);

        //    if (user == null)
        //        return NotFound();
        //    return View(user);

        //}
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            // Map the Patient model to ProfileViewModel
            var model = new Patient
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Age = user.Age,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender
            };

            // Pass ProfileViewModel to the view
            return View(model);
        }



        //[HttpPost]
        //public async Task<IActionResult> Edit(ProfileViewModel model)
        //{


        //    try
        //    {
        //        var existingPatient = await _userManager.FindByIdAsync(model.Id);
        //        if (existingPatient == null)
        //        {
        //            return NotFound(); // If patient doesn't exist
        //        }

        //        existingPatient.FirstName = model.FirstName;
        //        existingPatient.LastName = model.LastName;
        //        existingPatient.Email = model.Email;
        //        existingPatient.Age = model.Age;
        //        existingPatient.PhoneNumber = model.PhoneNumber;

        //        var result = await _userManager.UpdateAsync(existingPatient);

        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //        else
        //        {
        //            foreach (var error in result.Errors)
        //            {
        //                ModelState.AddModelError(string.Empty, error.Description);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, ex.Message);
        //    }
        //    return View(model);
        //}

        [HttpPost]
        public async Task<IActionResult> Edit(Patient patient)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(patient.Id);  // Now model.Id will contain the user ID
                if (user == null)
                {
                    return NotFound(); // If patient doesn't exist
                }

                user.FirstName = patient.FirstName;
                user.LastName = patient.LastName;
                user.Email = patient.Email;
                user.Age = patient.Age;
                user.PhoneNumber = patient.PhoneNumber;
                user.Gender = patient.Gender;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index","Home");
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
            return View(patient);
        }




        [HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid) 
			{
				var User = new Patient()
				{
					UserName = model.Email.Split('@')[0],
					Email = model.Email,
					FirstName = model.FName,
					LastName = model.LName, 
					Agree = model.Agree,
					Age = model.Age,
					PhoneNumber = model.PhoneNumber,
					Gender = model.Gender,
					
				};
				IdentityResult Result = await _userManager.CreateAsync(User, model.Password);

				if (Result.Succeeded)
				{ 
                    await _userManager.AddToRoleAsync(User, "Patient");
                    return RedirectToAction("Login","Account");
				}
				else 
					foreach (var error in Result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);

			}
			return View(model); 
		}




		[HttpGet]
		public IActionResult Login(string role)
		{
			// Check if the role is provided, if not return an error or redirect to role selection
			if (string.IsNullOrEmpty(role))
			{
				ModelState.AddModelError("", "Please select a role.");
				return RedirectToAction("ChooseRole", "Account"); // Assuming you have a role selection action
			}

			// Store the role in TempData so it can persist between requests
			TempData["Role"] = role;

			// Pass the role to the view in case it's needed there (e.g., for display purposes)
			ViewBag.Role = role;

			// Return the login view
			return View();
		}





        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string role)
        {
            
            if (string.IsNullOrEmpty(role))
            {
                role = TempData["Role"] as string;
            }

            
            if (string.IsNullOrEmpty(role))
            {
                ModelState.AddModelError("", "The role field is required.");
                TempData["Role"] = role; 
                return View(model);
            }

            TempData["Role"] = role; 

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    bool isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (isPasswordValid)
                    {
                        var roles = await _userManager.GetRolesAsync(user);

                        if (roles.Contains(role))
                        {
                            await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                            
                            if (role == "Admin")
                            {
                                return RedirectToAction("Index", "Admin"); 
                            }
                            else if (role == "Patient")
                            {
                                return RedirectToAction("Index", "Home", new { PatientId = user.Id }); 
                            }
                            else if (role == "Doctor")
                            {
                                return RedirectToAction("Details", "Doctor", new { DoctorId = user.Id }); 
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Access Denied: You do not have permission to access this area.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect Email or Password.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "This Email doesn't exist.");
                }
            }

            return View(model); // Stay on the same page with the error messages
        }




        public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login");
		}




		[HttpGet]
		public IActionResult ForgetPassword()
		{
			return View();
		}



		[HttpPost]
		public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var User = await _userManager.FindByEmailAsync(model.Email);
				if (User is not null) //check if this user exists
				{

					var token = await _userManager.GeneratePasswordResetTokenAsync(User); //valid for one time only & for this user only

					var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = User.Email, Token = token }, Request.Scheme); //Scheme: protocol+host+port, ex:https://localhost:5070

					Email email = new Email()
					{
						Subject = "Reset Password",
						To = model.Email,
						Body = ResetPasswordLink
					};
					EmailSettings.SendEmail(email);
					return RedirectToAction("CheckYourEmailInbox");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Email doesn't exist");

				}
			}
			return View("ForgetPassword", model); //will return this if ModelState in invalid, or if user doesn't exist

		}


		[HttpGet]
		public IActionResult CheckYourEmailInbox()
		{
			return View();
		}



		[HttpGet]
		public IActionResult ResetPassword(string email, string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				string email = TempData["email"] as string;
				string token = TempData["token"] as string;
				var User = await _userManager.FindByEmailAsync(email);
				var Result = await _userManager.ResetPasswordAsync(User, token, model.NewPassword);

				if (Result.Succeeded)
					//return RedirectToAction(nameof(Login));
					return RedirectToAction("ChooseRole", "Home");


				else
					foreach (var error in Result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);


			}

			return View(model);

		}




    }
}
