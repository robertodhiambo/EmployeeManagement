using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context;

        public SQLEmployeeRepository(AppDbContext _context)
        {
            context = _context;
        }

        public Employee Add(Employee employee)
        {
            context.Employee.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = context.Employee.Find(id);
            if (employee != null)
            {
                context.Employee.Remove(employee);
                context.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return context.Employee;
        }

        public Employee GetEmployee(int Id)
        {
          return context.Employee.Find(Id);
        }

        public Employee Update(Employee employeeChanges)
        {
            var employee = context.Employee.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return employeeChanges;
        }
    }
}
