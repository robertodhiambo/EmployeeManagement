namespace EmpManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        
         private List<Employee> _employeesList;

        public MockEmployeeRepository()
        {
            _employeesList = new List<Employee>()
            {
                 new Employee() {Id = 1, Name = "Mary", Department =Dept.HR , Email = "mary@gmail"},
                new Employee() {Id = 2, Name = "John", Department = Dept.IT, Email = "john@gmail"},
                new Employee() {Id = 3, Name = "Sam", Department = Dept.Payroll, Email = "sam@gmail"}
            };
        }

        public Employee Add ( Employee employee )
        {
            employee.Id = _employeesList.Max ( x => x.Id ) + 1;
            _employeesList.Add( employee );
            return employee;
        }

        public Employee Delete ( int id )
        {
            Employee employee = _employeesList.FirstOrDefault( employee=> employee.Id == id );    
            if ( employee != null )
            {
                _employeesList.Remove( employee );
            }

            return employee;
        }

        public IEnumerable<Employee> GetAllEmployee ( )
        {
            return _employeesList;
        }

        public Employee GetEmployee(int Id)
        {
             return _employeesList.FirstOrDefault(e => e.Id == Id);
        }

        public Employee Update ( Employee employeeChanges )
        {
            Employee employee = _employeesList.FirstOrDefault(employee=> employee.Id == employeeChanges.Id);
            if ( employee != null )
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }

            return employee;
        }
    }
}
