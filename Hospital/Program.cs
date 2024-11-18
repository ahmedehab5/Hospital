using Hospital.Contexts;
using Hospital.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<HospitalDBContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddIdentity<Person, IdentityRole>
(Options =>
{
    Options.Password.RequireNonAlphanumeric = true;
    Options.Password.RequireDigit = true;
    Options.Password.RequireLowercase = true;
    Options.Password.RequireUppercase = true;
})
                .AddEntityFrameworkStores<HospitalDBContext>()
                .AddDefaultTokenProviders();


//encryption schema
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(Options =>
{
    Options.LoginPath = "Account/Login"; //if token gets expired, return to login
    Options.AccessDeniedPath = "Home/Error"; //if you tried to access an action that you are not authorized to
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=ChooseRole}/{id?}");



using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Patient", "Doctor" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}


using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Person>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<HospitalDBContext>(); // Replace with your actual DbContext
    //Admin
    string AdminEmail = "admin@hospital.com";
    string AdminPassword = "Admin_1";
    if (await userManager.FindByEmailAsync(AdminEmail) == null)
    {
        var admin = new Admin
        {
            UserName = AdminEmail.Split('@')[0],
            Email = AdminEmail,
            FirstName = "Mohammed",
            LastName = "Ali",
            PhoneNumber = "01177334466",
            Gender=Gender.Male,
			DOB = DateOnly.Parse("1988-08-09"),
			Age =36
        };
        await userManager.CreateAsync(admin, AdminPassword);
        await userManager.AddToRoleAsync(admin, "Admin");

    }

    string specializationName = "Cardiology"; // Example specialization
    var specialization = await dbContext.Specializations.FirstOrDefaultAsync(s => s.Name == specializationName);
    if (specialization == null)
    {
        specialization = new Specialization
        {
            Name = specializationName
        };
        dbContext.Specializations.Add(specialization);
        await dbContext.SaveChangesAsync();
    }

    // Doctor
    string DoctorEmail = "doctor@gmail.com";
    string DoctorPassword = "Doctor_1";
    if (await userManager.FindByEmailAsync(DoctorEmail) == null)
    {
        var doctor = new Doctor
        {
            UserName = DoctorEmail.Split('@')[0],
            Email = DoctorEmail,
            FirstName = "Samy",
            LastName = "Mohammed",
            SpecializationId = specialization.Id,
            PhoneNumber="01122334455",
            WorkingDays = WeekDays.Sunday | WeekDays.Monday | WeekDays.Tuesday,
            Gender = Gender.Male,
            Age = 46,
			DOB = DateOnly.Parse("1978-08-09"),
			Salary =9000,
            StartTime = new TimeOnly(15, 0),  
            EndTime = new TimeOnly(23, 0)    

        };
        // Set image if it's uploaded and stored in wwwroot
        var imagePath = Path.Combine("wwwroot", "images", "dr1.png"); // Adjust path accordingly
        if (System.IO.File.Exists(imagePath))
        {
            doctor.Image = await System.IO.File.ReadAllBytesAsync(imagePath); // Convert image to byte array
        }
        await userManager.CreateAsync(doctor, DoctorPassword);
        await userManager.AddToRoleAsync(doctor, "Doctor");
    }

    // Patient
    string PatientEmail = "patient@gmail.com";
    string PatientPassword = "Patient_1";
    if (await userManager.FindByEmailAsync(PatientEmail) == null)
    {
        var patient = new Patient
        {
            UserName = PatientEmail.Split('@')[0],
            Email = PatientEmail,
            FirstName = "Abdelrahman",
            LastName = "Ali",
            PhoneNumber = "01122554499",
			DOB = DateOnly.Parse("2001-08-09"),
			Gender = Gender.Male,
            Age=23
        };
        await userManager.CreateAsync(patient, PatientPassword);
        await userManager.AddToRoleAsync(patient, "Patient");
    }


}





app.Run();
