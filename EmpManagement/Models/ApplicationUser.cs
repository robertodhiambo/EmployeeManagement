﻿using Microsoft.AspNetCore.Identity;

namespace EmpManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
    }
}
