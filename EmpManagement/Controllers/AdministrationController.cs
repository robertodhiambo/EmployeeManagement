﻿using EmpManagement.Models;
using EmpManagement.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EmpManagement.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController( RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser>
                                                                    userManager) 
        {
            this.roleManager=roleManager;
            this.userManager=userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                { 
                   Name = viewModel.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel 
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add ( user.UserName );
                }
            }

            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> EditRole (  EditRoleViewModel model )
        {
            var role = await roleManager.FindByIdAsync ( model.Id );

            if ( role == null )
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View ( "NotFound" );
            }

            else
            {
                role.Name = model.RoleName;

                var result = await roleManager.UpdateAsync( role );

                if ( result.Succeeded)
                {
                    return RedirectToAction ( "ListRoles" );
                }

                foreach (var error  in result.Errors)
                {
                    ModelState.AddModelError ( "" , error.Description );
                }

                return View ( model );
            }

        }
    }
}
