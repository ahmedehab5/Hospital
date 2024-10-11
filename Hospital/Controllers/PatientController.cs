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
        public IActionResult Create()
        {
            return View();
        }


        //[Authorize(Roles ="")]
        [HttpPost]
        public async Task<IActionResult> Create(Patient patient)
        {           

            if (ModelState.IsValid)
            {
                patient.UserName = patient.Email.Split('@')[0];
                patient.Agree = true;
                await _userManager.CreateAsync(patient);
                return RedirectToAction("index");

            }
            else

            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                ModelState.AddModelError(string.Empty, "Couldn't create patient");
                return View(patient);

            }
            
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



        [HttpGet]
        public async Task<IActionResult> Edit (string? id)
        {
            if(id == null)
                return NotFound();
            var patient = await _userManager.FindByIdAsync(id);

            if (patient == null)
                return NotFound();
            return View(patient);
                
        }



        [HttpPost]
        public async Task<IActionResult> Edit(Patient patient)
        {
           
            
            try
            {
                var existingPatient = await _userManager.FindByIdAsync(patient.Id);
                if (existingPatient == null)
                {
                    return NotFound(); // If patient doesn't exist
                }

                existingPatient.FirstName = patient.FirstName;
                existingPatient.LastName = patient.LastName;
                existingPatient.Email = patient.Email;
                existingPatient.Age = patient.Age;
                existingPatient.PhoneNumber = patient.PhoneNumber;

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
            return View(patient);
        }




        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
                return NotFound();

            var patient = await _userManager.FindByIdAsync(id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Patient patient)
        {
            try
            {
                var ToBeDeleted = await _userManager.FindByIdAsync(patient.Id);
                if (ToBeDeleted == null)
                {
                    return NotFound(); // If patient doesn't exist
                }

                await _userManager.DeleteAsync(ToBeDeleted);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(patient);
            }
        }

    }
}
