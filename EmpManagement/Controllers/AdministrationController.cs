﻿using EmpManagement.Models;
using EmpManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EmpManagement.Controllers
{
    [Authorize(Roles = "Admin")]
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
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
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

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync( roleId );

            if ( role == null )
            {
                ViewBag.ErroMessage = $"Role with Id = {roleId} cannot be found";
                return View ( "NotFound" );
            }

            var model = new List<UserRoleViewModel> ( );

            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel 
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }

                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add( userRoleViewModel );
            }

            return View ( model );
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync ( roleId );

            if ( role == null )
            {
                ViewBag.ErroMessage = $"Role with Id = {roleId} cannot be found";
                return View ( "NotFound" );
            }

            for (int i = 0 ; i < model.Count ; i++)
            {
                var user = await userManager.FindByIdAsync ( model [ i ].UserId );

                IdentityResult result = null;

                if ( model [i].IsSelected && ! (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }

                else if ( ! model [ i ].IsSelected &&  await userManager.IsInRoleAsync ( user , role.Name ) )
                {
                    result = await userManager.RemoveFromRoleAsync ( user , role.Name );
                }

                else
                {
                    continue; 
                }

                if (result.Succeeded)
                {
                    if ( i < ( model.Count - 1 ) )
                        continue;
                    else
                        return RedirectToAction ( "EditRole" , new { Id = roleId } );
                }
            }

            return RedirectToAction ( "EditRole" , new { Id = roleId } );
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync ( id ); 

            if (user == null )
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View ( "NotFound" );
            }

            var userClaims = await userManager.GetClaimsAsync ( user );
            var userRoles = await userManager.GetRolesAsync ( user );

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                City = user.City,
                Claims = userClaims.Select( c => c.Value ).ToList(),
                Roles = userRoles
            };

            return View ( model );
        }

        [HttpPost]
        public async Task<IActionResult> EditUser ( EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync ( model.Id );

            if ( user == null )
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View ( "NotFound" );
            }

            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.City = model.City;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction ( "ListUsers" );
                }

                foreach (var erorr in result.Errors)
                {
                    ModelState.AddModelError ( "" , erorr.Description );
                }

                return View ( model );
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync ( id);

            if ( user == null )
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View ( "NotFound" );
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction ( "ListUsers" );
                }

                foreach (var erorr in result.Errors)
                {
                    ModelState.AddModelError("", erorr.Description );
                }

                return View ( "ListUsers" );
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole ( string id )
        {
            var role = await roleManager.FindByIdAsync ( id );

            if ( role == null )
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View ( "NotFound" );
            }
            else
            {
                var result = await roleManager.DeleteAsync ( role );

                if ( result.Succeeded )
                {
                    return RedirectToAction ( "ListRoles" );
                }

                foreach ( var erorr in result.Errors )
                {
                    ModelState.AddModelError ( "" , erorr.Description );
                }

                return View ( "ListRoles" );
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            var user = await userManager.FindByIdAsync( userId );

            if ( user == null )
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View ( "NotFound" );
            }

            var model = new List<UserRolesViewModel> ( );

            foreach (var role in roleManager.Roles )
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id ,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }

                else
                {
                    userRolesViewModel.IsSelected= false;
                }

                model.Add( userRolesViewModel );
            }

            return View ( model );
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            var user = await userManager.FindByIdAsync ( userId );

            if ( user == null )
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View ( "NotFound" );
            }

            var roles = await userManager.GetRolesAsync ( user );
            var result = await userManager.RemoveFromRolesAsync(user, roles );

            if (!result.Succeeded)
            {
                ModelState.AddModelError ( "" , "Cannot remove user existing roles" );
                return View ( model );
            }

            result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError ( "" , "Cannot add selected roles to user" );
                return View ( model );
            }

            return RedirectToAction ( "EditUser" , new { Id = userId } );
        }
    }
}
