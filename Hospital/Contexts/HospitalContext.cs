using Hospital.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace Hospital.Contexts
{
    public class HospitalDBContext : IdentityDbContext<Person>
    {

		public HospitalDBContext(DbContextOptions<HospitalDBContext> options) : base(options)
		{

		}

       

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Specialization> Specializations { get; set; }


    }
}
