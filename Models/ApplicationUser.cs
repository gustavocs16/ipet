using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Nome { get; set; }
        public string Foto { get; set; }
    }
}
