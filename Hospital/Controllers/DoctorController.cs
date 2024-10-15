using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hospital.Contexts;
using Hospital.Models;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Controllers
{
    public class DoctorController : Controller
    {
        private readonly HospitalDBContext _context;
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;

        public DoctorController(HospitalDBContext context, UserManager<Person> userManager, SignInManager<Person> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Doctor
        public async Task<IActionResult> Index()
        {
            var hospitalDBContext = _context.Doctors.Include(d => d.Specialization);
            return View(await hospitalDBContext.ToListAsync());
        }

        //index for signed in doctor
        public async Task<IActionResult> IndexUser()
        {
            var doctor = await _context.Doctors.Include(d => d.Specialization).FirstOrDefaultAsync(d => d.Id == _userManager.GetUserId(User));
            return View(doctor);
        }

        // GET: Doctor/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.Specialization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        public async Task<IActionResult> DetailsUser()
        {
            var doctor = await _context.Doctors.Include(d => d.Specialization).FirstOrDefaultAsync(d => d.Id == _userManager.GetUserId(User));
            return View(doctor);
        }

        // GET: Doctor/Create
        public IActionResult Create()
        {
            ViewData["SpecializationId"] = new SelectList(_context.Specializations, "Id", "Name");
            var daysList = Enum.GetValues(typeof(WeekDays))
                        .Cast<WeekDays>()
                        .Select(d => new { Id = (int)d, Name = d.ToString() })
                        .ToList();
            ViewData["WeekDays"] = daysList; // Pass the list of days directly
            return View();
        }

        // POST: Doctor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,WorkingDays,StartTime,EndTime,SpecializationId,Salary,Id,FirstName,LastName,Age,PhoneNumber,Gender,PasswordHash,Email,Image")] Doctor doctor,[Bind("ImageData")] string ImageData = null)
        {
            doctor.Image = ImageData != null ? ImageData.Split(',').Select(byte.Parse).ToArray():null;
            //if (ModelState.IsValid)
            {
                doctor.Agree = true;
                IdentityResult Result = await _userManager.CreateAsync(doctor, doctor.PasswordHash);

                if (Result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(doctor, "Doctor");
                }
                else
                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
          
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["SpecializationId"] = new SelectList(_context.Specializations, "Id", "Id", doctor.SpecializationId);
            return View(doctor);
        }

        // GET: Doctor/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            var daysList = Enum.GetValues(typeof(WeekDays))
                        .Cast<WeekDays>()
                        .Select(d => new { Id = (int)d, Name = d.ToString() })
                        .ToList();
            ViewData["WeekDays"] = daysList; // Pass the list of days directly
            if (doctor == null)
            {
                return NotFound();
            }
            ViewData["SpecializationId"] = new SelectList(_context.Specializations, "Id", "Name", doctor.SpecializationId);
            return View(doctor);
        }

        // POST: Doctor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,WorkingDays,StartTime,EndTime,SpecializationId,Salary,Id,FirstName,LastName,Age,PhoneNumber,Gender,PasswordHash,Email,Image")] Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }
            string? ImageData = Request.Form["ImageData"];
            doctor.Image = ImageData != "" ? ImageData.Split(',').Select(byte.Parse).ToArray() : null;
            //if (ModelState.IsValid)
            {
                try
                {
                    doctor.ConcurrencyStamp = _context.Doctors.AsNoTracking().FirstOrDefault(d => d.Id == id).ConcurrencyStamp;
                    doctor.Agree = true;
                    doctor.LockoutEnabled = true;
                    doctor.NormalizedEmail = doctor.Email.ToUpper();
                    
                    string oldPassword = _context.Doctors.AsNoTracking().FirstOrDefault(d => d.Id == id).PasswordHash;
                    if (doctor.PasswordHash != oldPassword)
                    {
                        _context.Update(doctor);
                        var token = await _userManager.GeneratePasswordResetTokenAsync(doctor);
                        await _userManager.ResetPasswordAsync(doctor, token, doctor.PasswordHash);
                    }
                    else
                    {
                        _context.Update(doctor);
                        await _context.SaveChangesAsync();
                    }
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpecializationId"] = new SelectList(_context.Specializations, "Id", "Id", doctor.SpecializationId);
            var daysList = Enum.GetValues(typeof(WeekDays))
                        .Cast<WeekDays>()
                        .Select(d => new { Id = (int)d, Name = d.ToString() })
                        .ToList();
            ViewData["WeekDays"] = daysList; // Pass the list of days directly
            return View(doctor);
        }

        public async Task<IActionResult> EditUser(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return View(doctor);
        }

        // POST: Doctor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id ,[Bind("Id,FirstName,LastName,PhoneNumber, PasswordHash,Image")] Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }

            string? ImageData = Request.Form["ImageData"];
          
            try
            {
                user.FirstName = doctor.FirstName;
                user.LastName = doctor.LastName;
                user.PhoneNumber = doctor.PhoneNumber;
                if(doctor.PasswordHash != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, doctor.PasswordHash);
                }
                var result = await _userManager.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(doctor.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Doctor/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.Specialization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(string id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }
}
