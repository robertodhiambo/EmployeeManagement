using Microsoft.AspNetCore.Mvc;

namespace EmpManagement.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet]
        public IActionResult Register ( )
        {
            return View ( );
        }
    }
}
