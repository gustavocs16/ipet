using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.Models
{
    public class Empresa
    {

        [Key]
        public int Id { get; set; }

        public string nomefantasia { get; set; }

        public string cnpj { get; set; }

        public string razaosocial { get; set; }

        public string email { get; set; }

        public string telefone { get; set; }

        public int responsavel { get; set; }

    }
}
