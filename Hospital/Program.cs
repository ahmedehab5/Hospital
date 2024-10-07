using Hospital.Contexts;
using Hospital.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<HospitalDBContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddIdentity<Person, IdentityRole>()
////server-side validations
//(Options =>
//{
//    Options.Password.RequireNonAlphanumeric = true;
//    Options.Password.RequireDigit = true;
//    Options.Password.RequireLowercase = true;
//    Options.Password.RequireUppercase = true;
//})
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
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
