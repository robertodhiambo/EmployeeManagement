using EmpManagement.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmpManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, 
                                                        SignInManager<IdentityUser> signInManager)
        {
            this.userManager=userManager;
            this.signInManager=signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Logout ( )
        {
            await signInManager.SignOutAsync();
            return RedirectToAction ( "Index" , "Home" );
        }

        [HttpGet]
        public IActionResult Register ( )
        {
            return View ( );
        }

        [HttpPost]
        public async Task<IActionResult> Register (RegisterViewModel viewModel )
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser 
                {
                    UserName = viewModel.Email,
                    Email = viewModel.Email
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
    }
}
