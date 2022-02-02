using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployees();
            return View(model);
        }

        [AllowAnonymous]
        public ViewResult Details(int id)
        {
            Employee model = _employeeRepository.GetEmployee(id);
            return View(model);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department
            };
            return View(employeeEditViewModel); 
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {

            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;

                _employeeRepository.Update(employee);
                return RedirectToAction("index");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        { 
          
            if (ModelState.IsValid)
            {
                Employee newEmployee = new Employee
                {
                    Name = employee.Name,
                    Email = employee.Email,
                    Department = employee.Department,
                };

                _employeeRepository.Add(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.Id });
            }

            return View();
        }
    }
}
