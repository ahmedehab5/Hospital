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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // sever name : AHMED-EHAB , database name : Hospital , integrated security : true, server certificate : true, windows authentication : true, authentication : windows
            optionsBuilder.UseSqlServer("Server=.;Database=Hospitall;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True");

            base.OnConfiguring(optionsBuilder);
        }


        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Specialization> Specializations { get; set; }


    }
}
