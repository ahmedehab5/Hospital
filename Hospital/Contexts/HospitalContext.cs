using Hospital.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Contexts
{
    public class HospitalDBContext : DbContext
    {
        public HospitalDBContext(DbContextOptions<HospitalDBContext> options): base(options) { }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // sever name : AHMED-EHAB , database name : Hospital , integrated security : true, server certificate : true, windows authentication : true, authentication : windows
            optionsBuilder.UseSqlServer("Server=AHMED-EHAB;Database=Hospitall;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True");

            base.OnConfiguring(optionsBuilder);
        }


        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Specialization> Specializations { get; set; }


    }
}
