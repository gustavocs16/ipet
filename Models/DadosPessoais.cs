using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("VWPessoa_Dado")]
    public class DadosPessoais
    {
        [Key]
        public int id { get; set; }

        public string Nome { get; set; }
        public string cpf { get; set; }
        public string data_nascimento { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
      


    }
}
