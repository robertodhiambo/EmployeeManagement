using System.ComponentModel.DataAnnotations;

namespace EmpManagement.ViewModel
{
    public class CreateRoleViewModel
    {
        [Required]
        public string  RoleName { get; set; }
    }
}
