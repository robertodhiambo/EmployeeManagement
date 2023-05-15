using EmpManagement.Models;
using EmpManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO.Enumeration;

namespace EmpManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, 
                                                        SignInManager<ApplicationUser> signInManager)
        {
            this.userManager=userManager;
            this.signInManager=signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register ( )
        {
            return View ( );
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register (RegisterViewModel viewModel )
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser 
                {
                    UserName = viewModel.Email,
                    Email = viewModel.Email,
                    City = viewModel.City
                };

                var result = await userManager.CreateAsync (user, viewModel.Password );

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View ( viewModel );
        }

        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json ( true );
            }
            
            else
            {
                return Json ( $"Email {email} is already in use" );
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login ( )
        {
            return View ( );
        }
         
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login (LoginViewModel viewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return  Redirect(returnUrl);
                    }

                    else
                    {
                        return RedirectToAction ("Index", "Home"); 
                    }

                }

                ModelState.AddModelError ( string.Empty , "Invalid Login Attempt" );
            }

            return View ( viewModel ); 
        }

        [HttpPost]
        public async Task<IActionResult> Logout ( )
        {
            await signInManager.SignOutAsync ( );
            return RedirectToAction("Index", "Home");
        }
    }
}
