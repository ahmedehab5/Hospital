using Hospital.Models;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace Hospital.Controllers
{
    public class RoleController : Controller
    {
        
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            
            _roleManager = roleManager;
        }


        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            // Map IdentityRole to RoleViewModel
            var roleViewModels = roles.Select(r => new RoleViewModel
            {
                Id = r.Id,
                RoleName = r.Name
            }).ToList();

            // Pass the RoleViewModel list to the view
            return View(roleViewModels);

        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {


            if (ModelState.IsValid)
            {
              
                var identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                // Create the role using RoleManager
                var result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }



        public async Task<IActionResult> Details(string id)
        {
            if(id == null)
                return NotFound();
            var Role= await _roleManager.FindByIdAsync(id);
            if (Role == null)
                return NotFound();
            var roleViewModel = new RoleViewModel
            {
                Id = Role.Id,
                RoleName = Role.Name
            };

            // Pass RoleViewModel to the view
            return View(roleViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
                return NotFound();
            var Role = await _roleManager.FindByIdAsync(id);
            if (Role == null)
                return NotFound();
            var roleViewModel = new RoleViewModel
            {
                Id = Role.Id,
                RoleName = Role.Name
            };

            // Pass RoleViewModel to the view
            return View(roleViewModel);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {
            try
            {
                var Role = await _roleManager.FindByIdAsync(model.Id);
                if (Role == null)
                {
                    return NotFound(); // If patient doesn't exist
                }

                Role.Name=model.RoleName;

                await _roleManager.UpdateAsync(Role);
                return RedirectToAction("Index");

               
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();
            var Role = await _roleManager.FindByIdAsync(id);
            if (Role == null)
                return NotFound();
            var roleViewModel = new RoleViewModel
            {
                Id = Role.Id,
                RoleName = Role.Name
            };

            // Pass RoleViewModel to the view
            return View(roleViewModel);
        }




        [HttpPost]
        public async Task<IActionResult> Delete(RoleViewModel model)
        {
            try
            {
                var Role = await _roleManager.FindByIdAsync(model.Id);
                if (Role == null)
                {
                    return NotFound(); // If patient doesn't exist
                }

                await _roleManager.DeleteAsync(Role);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }




       

    }
}
