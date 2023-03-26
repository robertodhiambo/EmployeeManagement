using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace EmpManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required, MaxLength(50, ErrorMessage ="Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }
        public Dept? Department { get; set; }
    }
}
