using EmpManagement.Models;
using EmpManagement.ViewModel;
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

        public ViewResult Details ( int? id)
        {
            Employee employee = _employeeRepository.GetEmployee ( id.Value );

            if (employee == null)
            {
                Response.StatusCode = 404;
                return View ( "EmployeeNotFound", id.Value );
            }

            Employee model = _employeeRepository.GetEmployee(id ?? 1);
            return View(model);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View ( );
        }

        [HttpPost]
        public IActionResult Create (EmployeeCreateViewModel model )
        {
            if (ModelState.IsValid)
            {
                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department
                };

                _employeeRepository.Add(newEmployee);

                return RedirectToAction ( "details" , new { id = newEmployee.Id } );
            }

            return View ( );

        }

        [HttpGet]
        public ViewResult Edit ( int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id ,
                Name = employee.Name ,
                Email = employee.Email ,
                Department = employee.Department
            };

            return View ( employeeEditViewModel ); 
        }

        [HttpPost]
        public IActionResult Edit( EmployeeEditViewModel viewModel )
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(viewModel.Id);
                employee.Name = viewModel.Name;
                employee.Email = viewModel.Email;
                employee.Department = viewModel.Department;

                _employeeRepository.Update(employee);
                return RedirectToAction ( "Index" );
            }

            return View ( );
        }

    }
}