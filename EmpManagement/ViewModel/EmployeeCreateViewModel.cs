using EmpManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace EmpManagement.ViewModel
{
    public class EmployeeCreateViewModel
    {
        [Required, MaxLength ( 50 , ErrorMessage = "Name cannot exceed 50 characters" )]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public Dept? Department { get; set; }
    }
}
