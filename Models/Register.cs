using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class Register
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [Remote(action:"IsEmailValid", controller:"Account")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password", 
            ErrorMessage ="Password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
