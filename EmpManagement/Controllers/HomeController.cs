using EmpManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmpManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployee ( );
            return View ( model );
        }

        public ViewResult Details ( int id)
        {
            Employee model = _employeeRepository.GetEmployee(id);
           
            return View(model);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View ( );
        }

        [HttpPost]
        public IActionResult Create (Employee employee )
        {
            if (ModelState.IsValid)
            {
                Employee newEmployee = _employeeRepository.Add ( employee );
                return RedirectToAction ( "details" , new { id = newEmployee.Id } );
            }

            return View ( );

        }

    }
}