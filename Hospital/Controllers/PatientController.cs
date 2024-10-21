using Hospital.Contexts;
using Hospital.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Hospital.Controllers
{
    public class PatientController : Controller
    {

        private readonly UserManager<Person> _userManager;

        public PatientController(UserManager<Person> userManager)
        {         
            _userManager = userManager;
        }


      
        public IActionResult Index()
        {
            var patients = _userManager.Users.OfType<Patient>().ToList();
            //var patients= _userManager.Users.ToList();
            return View(patients);
        }




        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
                return NotFound();

            var patient = await _userManager.FindByIdAsync(id);

            if (patient == null)
                return NotFound();

            return View (patient);
        }





    }
}
