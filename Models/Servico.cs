using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Servico
    {
        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }
        public string Descricao { get; set; }

    }
}
