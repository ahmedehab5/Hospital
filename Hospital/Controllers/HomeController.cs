using Hospital.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Hospital.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

		//[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		//public IActionResult Error()
		//{
		//    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		//}

		public IActionResult Error()
		{
			// You can pass a custom error message to the view if you want
			ViewBag.ErrorMessage = "You do not have permission to access this page.";
			return View();
		}


        public IActionResult ChooseRole(string activeRole)
        {
            ViewData["ActiveRole"] = activeRole;
            return View();
        }

	}
}
