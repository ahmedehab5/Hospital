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
                    return RedirectToAction("Login");
				}
				else 
					foreach (var error in Result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);

			}
			return View(model); 
		}



		#region LAST
		//[HttpGet]
		//public IActionResult Login()
		//{
		//	return View();
		//} 
		#endregion


		#region LAST2
		//public IActionResult Login(string role)
		//{
		//    if (string.IsNullOrEmpty(role))
		//    {
		//        // Handle the case when the role is not provided (error handling or redirect)
		//        ModelState.AddModelError("", "The role field is required.");
		//        return View();
		//    }

		//    // Pass the role to the view so it can be included in the form
		//    ViewBag.Role = role;
		//    return View();
		//} 
		#endregion

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



		//[Authorize(Roles = "Patient")]
		//[HttpPost]
		//public async Task<IActionResult> Login(LoginViewModel model)
		//{
		//	if (ModelState.IsValid)
		//	{
		//              var user = await _userManager.FindByEmailAsync(model.Email);

		//		if (user != null) //check if user exists
		//		{
		//			bool Flag = await _userManager.CheckPasswordAsync(user, model.Password);
		//			if (Flag) //check password
		//			{
		//				var Result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
		//				return RedirectToAction("Index", "Home");
		//			}

		//			else
		//				ModelState.AddModelError("", "Incorrect Email or Password");
		//		}
		//		else //if this user doesn't exist
		//		{
		//			ModelState.AddModelError(string.Empty, "This Email doesn't exist");
		//		}
		//	}

		//	return View(model);
		//}


		//[Authorize(Roles = "Patient")]
		//[HttpPost]
		//public async Task<IActionResult> Login(LoginViewModel model)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        var user = await _userManager.FindByEmailAsync(model.Email);

		//        if (user != null) //check if user exists
		//        {
		//            if (await _userManager.IsInRoleAsync(user, "Patient"))
		//            {
		//                bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);
		//                if (isPasswordCorrect) //check password
		//                {
		//                    await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
		//                    return RedirectToAction("Index", "Home");
		//                }
		//                else
		//                {
		//                    ModelState.AddModelError("", "Incorrect Email or Password");
		//                }
		//            }
		//            else
		//            {
		//                ModelState.AddModelError("", "Access Denied: You are not authorized to login as a patient.");
		//            }
		//        }
		//        else //if this user doesn't exist
		//        {
		//            ModelState.AddModelError(string.Empty, "This Email doesn't exist");
		//        }
		//    }

		//    return View(model);
		//}


		#region MyRegion
		//[Authorize(Roles = "Patient")]
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
		//                if (roles.Contains("Patient"))
		//                {
		//                    // Sign in the admin and redirect to the admin dashboard
		//                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
		//                    return RedirectToAction("Index", "Account");
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

		#region LAST
		////[Authorize(Roles = "Patient")]
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
		//            if (roles.Contains("Patient"))
		//            {
		//                // Check if the password is correct
		//                bool isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
		//                if (isPasswordValid) // If password is valid, sign in
		//                {
		//                    await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
		//                    return RedirectToAction("Index", "Home"); // Redirect to Admin dashboard
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



		#region LAST2
		//[HttpPost]
		//public async Task<IActionResult> Login(LoginViewModel model, string role)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        var user = await _userManager.FindByEmailAsync(model.Email);

		//        if (user != null) // Check if user exists
		//        {
		//            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
		//            if (isPasswordValid)
		//            {
		//                var roles = await _userManager.GetRolesAsync(user);

		//                if (roles.Contains(role)) // Check if the user has the selected role
		//                {
		//                    await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

		//                    // Redirect based on role
		//                    if (role == "Admin")
		//                        return RedirectToAction("Index", "Admin");
		//                    else if (role == "Patient")
		//                        return RedirectToAction("Index", "Patient");
		//                    else if (role == "Doctor")
		//                        return RedirectToAction("Index", "Doctor");
		//                }
		//                else
		//                {
		//                    ModelState.AddModelError(string.Empty, $"Access Denied: You do not have {role} permissions.");
		//                }
		//            }
		//            else
		//            {
		//                ModelState.AddModelError("", "Incorrect Password.");
		//            }
		//        }
		//        else
		//        {
		//            ModelState.AddModelError("", "This Email doesn't exist.");
		//        }
		//    }

		//    return View(model);
		//} 
		#endregion


		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model, string role)
		{
			// If role is not provided, attempt to retrieve it from TempData
			if (string.IsNullOrEmpty(role))
			{
				role = TempData["Role"] as string;
			}

			// If the role is still missing, return an error
			if (string.IsNullOrEmpty(role))
			{
				ModelState.AddModelError("", "The role field is required.");
				TempData["Role"] = role; // Store the role in TempData for next attempt
				return View(model);
			}

			TempData["Role"] = role; // Store the role in TempData for next attempt

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
							return RedirectToAction("Index", role == "Admin" ? "Admin" : "Home");
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



        //[HttpGet]
        //public IActionResult AccessDenied()
        //{
        //    return View(); // This will look for a view named AccessDenied.cshtml
        //}

    }
}
