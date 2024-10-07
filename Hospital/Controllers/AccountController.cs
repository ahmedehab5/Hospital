using Hospital.Models;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<Person> _userManager;
		private readonly SignInManager<Person> _signInManager;

		public AccountController(UserManager<Person> userManager, SignInManager<Person> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
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
				Person User = new Person()
				{
					UserName = model.Email.Split('@')[0],
					Email = model.Email,
					FirstName = model.FName,
					LastName = model.LName, //LastName:Person  LName:RegisterViewModel
					IsAgree = model.IsAgree,
					Age=model.Age,
					PhoneNumber=model.PhoneNumber,
					Gender=model.Gender,
					PasswordHash=model.Password
				};

				IdentityResult Result= await _userManager.CreateAsync(User, model.Password);

				if (Result.Succeeded) //if registered successfully, Redirect to Login action				
					//return RedirectToAction("Login");
					await _signInManager.SignInAsync(User,false); //sign in directly, without Redirecting to Login action & require email&password again

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
						var Result=await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
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
			
			return View(model) ;
		}


		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login");
		}

	}
}
