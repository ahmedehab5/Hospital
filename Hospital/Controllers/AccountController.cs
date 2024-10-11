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
            _roleManager = roleManager;
        }


		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid) //server-side validation ,, check if there is any required field that is not filled
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
					//PasswordHash = model.Password
				};

				IdentityResult Result = await _userManager.CreateAsync(User, model.Password);

				if (Result.Succeeded)
				{ 
                    await _userManager.AddToRoleAsync(User, "Patient");
                    return RedirectToAction("Login");
					//await _signInManager.SignInAsync(User, false); //sign in directly, without Redirecting to Login action & require email&password again
				}
				else //if not, return the errors
					foreach (var error in Result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);

			}
			return View(model); //in case of Invalid or Not succeeded, return to the same view again
		}



		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}



		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
                Person user = await _userManager.FindByEmailAsync(model.Email);

				if (user != null) //check if user exists
				{
					bool Flag = await _userManager.CheckPasswordAsync(user, model.Password);
					if (Flag) //check password
					{
						var Result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
						return RedirectToAction("Index", "Home");
					}

					else
						ModelState.AddModelError("", "Incorrect Email or Password");
				}
				else //if this user doesn't exist
				{
					ModelState.AddModelError(string.Empty, "This Email doesn't exist");
				}
			}

			return View(model);
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
					return RedirectToAction(nameof(Login));

				else
					foreach (var error in Result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);


			}

			return View(model);

		}

	}
}
