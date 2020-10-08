using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string data_nascimento { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Tipo_usuario { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string foto_perfil { get; set; }
        public IFormFile Image { get; set; }


    }
}
