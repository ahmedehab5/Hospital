using Hospital.Models;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<Person> _userManager;

		public AccountController(UserManager<Person> userManager)
        {
			_userManager = userManager;
		}


        public IActionResult Register()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid) //server-side validation ,, check if there is any required field that is not filled
			{
				var User = new Person()
				{
					UserName = model.Email.Split('@')[0],
					Email = model.Email,
					FirstName = model.FName,
					LastName = model.LName, //LastName:Person  LName:RegisterViewModel
					IsAgree = model.IsAgree,
					Age=model.Age,
					PhoneNumber=model.PhoneNumber,
					Gender=model.Gender
				};

				var Result= await _userManager.CreateAsync(User, model.Password);
				
				if (Result.Succeeded) //if registered successfully, Redirect to Login action				
					return RedirectToAction("Login");
				
				else //if not, return the errorsw
					foreach(var error in Result.Errors)					
						ModelState.AddModelError(string.Empty, error.Description);
						
			}
			return View(model); //in case of Invalid or Not succeeded, return to the same view again
		}


		public IActionResult Login()
		{
			return View();
		}

	}
}
