using Hospital.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Reflection.Emit;

namespace Hospital.Contexts
{
    public class HospitalDBContext : IdentityDbContext<Person>
    {

		public HospitalDBContext(DbContextOptions<HospitalDBContext> options) : base(options)
		{

		}

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Patient> Patients { get; set; }
		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Appointment> Appointments { get; set; }
		public DbSet<Specialization> Specializations { get; set; }


		//protected override void OnModelCreating(ModelBuilder builder)
		//{

		//	base.OnModelCreating(builder);

		//	builder.Entity<Doctor>().ToTable("Doctors");
		//	builder.Entity<Patient>().ToTable("Patients");


		//	builder.Entity<Person>(entity =>
		//	{
		//		entity.HasKey(e => e.Id); 
		//	});


		//	builder.Entity<Doctor>()
		//	.ToTable("Doctors")
		//	.HasBaseType<Person>();


		//	builder.Entity<Patient>()
		//	.ToTable("Patients")
		//	.HasBaseType<Person>();
		//}




	}
}
