using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hospital.Contexts;
using Hospital.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hospital.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly HospitalDBContext _context;

        public AppointmentController(HospitalDBContext context)
        {
            _context = context;
        }

        #region Get All Appointments

        // GET: Appointment
     
        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Select(a => new
                {
                    a.Id,
                    DoctorFullName = a.Doctor.FirstName + " " + a.Doctor.LastName, // Combine FirstName and LastName
                    PatientFullName = a.Patient.FirstName + " " + a.Patient.LastName, // Combine FirstName and LastName
                    a.Specialization,
                    a.Diagnosis,
                    a.Treatment,
                    a.Date
                })
                .ToListAsync();

            return View(appointments);
        }
        #endregion

        #region DetailsForAppointment
        // GET: Appointment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }
        #endregion

        #region HistoryForPatient
        // GET: Appointment/History/Patient/{patientId}
        [HttpGet]
        public IActionResult HistoryForPatient(string patientId)
        {
            // Retrieve patient information
            var patient = _context.Patients
                .Where(p => p.Id == patientId)
                .Select(p => new { p.FirstName, p.LastName })
                .FirstOrDefault();

            if (patient == null)
            {
                return NotFound(); // Handle case where patient is not found
            }

            // Retrieve past appointments for the patient (appointments before today)
            var appointments = _context.Appointments
                .Where(a => a.PatientId == patientId && a.Date < DateOnly.FromDateTime(DateTime.Now.Date)) // Filter out future appointments
                .Include(a => a.Doctor) // Include doctor details
                .OrderByDescending(a => a.Date)
                .ToList();

            // Pass patient name to the view
            ViewData["PatientName"] = $"{patient.FirstName} {patient.LastName}";

            return View(appointments);
        }

        #endregion

        #region UpcomingAppointmentsForPatient
        [HttpGet]
        public IActionResult UpcomingAppointmentsForPatient(string patientId)
        {
            // Retrieve patient information
            var patient = _context.Patients
                .Where(p => p.Id == patientId)
                .Select(p => new { p.FirstName, p.LastName })
                .FirstOrDefault();

            if (patient == null)
            {
                return NotFound(); // Handle case where patient is not found
            }

            // Retrieve upcoming appointments for the patient
            var appointments = _context.Appointments
                .Where(a => a.PatientId == patientId && a.Date >= DateOnly.FromDateTime(DateTime.Now.Date)) // Show only future appointments
                .Include(a => a.Doctor) // Include doctor details
                .OrderBy(a => a.Date)
                .ToList();

            // Pass patient name to the view
            ViewData["PatientName"] = $"{patient.FirstName} {patient.LastName}";

            return View(appointments);
        }

        #endregion

        #region HistoryForDoctor
        // GET: Appointment/History/Doctor/{doctorId}
        [HttpGet]
        public IActionResult HistoryForDoctor(string doctorId)
        {
            // Retrieve doctor information
            var doctor = _context.Doctors
                .Where(d => d.Id == doctorId)
                .Select(d => new { d.FirstName, d.LastName })
                .FirstOrDefault();

            if (doctor == null)
            {
                return NotFound(); // Handle case where doctor is not found
            }

            // Retrieve appointments for the doctor that are before today
            var appointments = _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.Date < DateOnly.FromDateTime(DateTime.Now.Date)) // Show only past appointments
                .Include(a => a.Patient) // Include patient details
                .OrderByDescending(a => a.Date)
                .ToList();

            // Pass doctor name to the view
            ViewData["DoctorName"] = $"{doctor.FirstName} {doctor.LastName}";

            return View(appointments);
        }

        #endregion





        #region UpcomingAppointmentsForDoctor
        [HttpGet]
        public IActionResult UpcomingAppointmentsForDoctor([Bind("Id")] string doctorId)
        {
            // Retrieve doctor information
            var doctor = _context.Doctors
                .Where(d => d.Id == doctorId)
                .Select(d => new { d.FirstName, d.LastName })
                .FirstOrDefault();

            if (doctor == null)
            {
                return NotFound(); // Handle case where doctor is not found
            }

            // Retrieve upcoming appointments for the doctor
            var appointments = _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.Date >= DateOnly.FromDateTime(DateTime.Now.Date)) // Show only future appointments
                .Include(a => a.Patient) // Include patient details
                .OrderBy(a => a.Date)
                .ToList();

            // Pass doctor name to the view
            ViewData["DoctorName"] = $"{doctor.FirstName} {doctor.LastName}";

            return View(appointments);
        }

        #endregion

        #region MakeAppointment(PatientSide)

        [Authorize(Roles ="Patient")]
        public IActionResult Create(string PatientId)
        {
            // Fetch specializations
            var specializations = _context.Specializations
                .Select(s => new { Id = s.Id, Name = s.Name })
                .ToList();
            ViewData["Specialization"] = new SelectList(specializations, "Name", "Name");

            // Populate Patient full names
            var patients = _context.Patients
                .Select(p => new { Id = p.Id, FullName = p.FirstName + " " + p.LastName })
                .ToList();
            ViewData["PatientId"] = new SelectList(patients, "Id", "FullName", PatientId);

            return View();
        }

        public async Task<IActionResult> GetAvailableSlots(string doctorId, DateOnly selectedDate)
        {
            // Retrieve the doctor by the ID
            var doctor = await _context.Doctors.FindAsync(doctorId);
            if (doctor == null)
            {
                return Json(new { error = "Doctor not found" });
            }

            // Get the start and end times for the doctor
            TimeOnly startTime = doctor.StartTime; // Assuming these are TimeOnly
            TimeOnly endTime = doctor.EndTime; // Assuming these are TimeOnly

            // Generate 20-minute slots between the start time and end time
            List<string> availableSlots = new List<string>();
            TimeSpan slotDuration = TimeSpan.FromMinutes(20);

            // Loop to generate time slots
            for (var time = startTime; time < endTime; time = time.Add(slotDuration))
            {
                string slot = $"{time.Hour:D2}:{time.Minute:D2} - {(time.Add(slotDuration).Hour):D2}:{(time.Add(slotDuration).Minute):D2}";
                availableSlots.Add(slot);
            }

            // Check for existing appointments for that doctor on the selected date
            var existingAppointments = await _context.Appointments
              .Where(a => a.DoctorId == doctorId && a.Date == selectedDate) // Direct comparison
              .ToListAsync();

            // Remove any slots that have already been reserved
            foreach (var appointment in existingAppointments)
            {
                availableSlots.Remove(appointment.Slot); // Ensure Slot is in the same format as generated above
            }

            return Json(availableSlots);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DoctorId,PatientId,Specialization,Diagnosis,Treatment,Date,Slot")] Appointment appointment)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                // Repopulate the ViewData in case of validation errors
                var doctors = await _context.Doctors.ToListAsync();
                var patients = await _context.Patients.ToListAsync();

                ViewData["DoctorId"] = new SelectList(doctors, "Id", "Name", appointment.DoctorId);
                ViewData["PatientId"] = new SelectList(patients, "Id", "Name", appointment.PatientId);

                return View(appointment); // Return view with validation errors
            }

            // Retrieve the Doctor based on the selected DoctorId
            var doctor = await _context.Doctors.FindAsync(appointment.DoctorId);
            if (doctor == null)
            {
                ModelState.AddModelError("DoctorId", "Selected doctor does not exist.");

                // Repopulate the ViewData before returning to the view
                var doctors = await _context.Doctors.ToListAsync();
                var patients = await _context.Patients.ToListAsync();

                ViewData["DoctorId"] = new SelectList(doctors, "Id", "Name", appointment.DoctorId);
                ViewData["PatientId"] = new SelectList(patients, "Id", "Name", appointment.PatientId);

                return View(appointment);
            }

            // Assign the retrieved doctor to the appointment
            appointment.Doctor = doctor;

            // Similarly retrieve the Patient based on the selected PatientId
            var patient = await _context.Patients.FindAsync(appointment.PatientId);
            if (patient == null)
            {
                ModelState.AddModelError("PatientId", "Selected patient does not exist.");

                // Repopulate the ViewData before returning to the view
                var doctors = await _context.Doctors.ToListAsync();
                var patients = await _context.Patients.ToListAsync();

                ViewData["DoctorId"] = new SelectList(doctors, "Id", "Name", appointment.DoctorId);
                ViewData["PatientId"] = new SelectList(patients, "Id", "Name", appointment.PatientId);

                return View(appointment);
            }

            // Assign the retrieved patient to the appointment
            appointment.Patient = patient;

            // Add the appointment to the database and save changes
            _context.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("UpcomingAppointmentsForPatient", "Appointment", new { patientId = appointment.PatientId }); // Redirect to Index after saving
        }

        [HttpGet]
        public JsonResult GetDoctorsBySpecialization(string specialization)
        {
            var doctors = _context.Doctors
                .Where(d => d.Specialization.Name == specialization)
                .Select(d => new
                {
                    Id = d.Id,
                    FullName = d.FirstName + " " + d.LastName,
                    WorkingDays = d.WorkingDays.ToString() // Assuming it returns a string format like "Saturday, Sunday"
                })
                .ToList();

            return Json(doctors);
        }

        #endregion

        #region Treatment&Diagnosis(DoctorSide)
        // GET: Appointment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }
            // Populate Doctor and Patient select lists with full names
            var doctors = _context.Doctors.Select(d => new
            {
                Id = d.Id,
                FullName = d.FirstName + " " + d.LastName
            }).ToList();

            var patients = _context.Patients.Select(p => new
            {
                Id = p.Id,
                FullName = p.FirstName + " " + p.LastName
            }).ToList();

            ViewData["DoctorId"] = new SelectList(doctors, "Id", "FullName", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(patients, "Id", "FullName", appointment.PatientId);

            return View(appointment);
        }

        // POST: Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DoctorId,PatientId,Specialization,Diagnosis,Treatment,Date,Slot")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("UpcomingAppointmentsForDoctor", "Appointment", new { doctorId = appointment.DoctorId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    ModelState.AddModelError(string.Empty, "Unable to save changes. The appointment was modified by another user.");
                }
            }
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
                    }
                }
            }

            // Repopulate select lists if the model state is invalid
            var doctors = _context.Doctors.Select(d => new
            {
                Id = d.Id,
                FullName = d.FirstName + " " + d.LastName
            }).ToList();

            var patients = _context.Patients.Select(p => new
            {
                Id = p.Id,
                FullName = p.FirstName + " " + p.LastName
            }).ToList();

            ViewData["DoctorId"] = new SelectList(doctors, "Id", "FullName", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(patients, "Id", "FullName", appointment.PatientId);

            return View(appointment);
        }
        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
        #endregion

        #region CancelAppointment
        // GET: Appointment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("UpcomingAppointmentsForPatient", "Appointment", new { patientId = appointment.PatientId });
        }
        #endregion

















    }
}
