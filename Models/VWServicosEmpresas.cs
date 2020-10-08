using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("VWServicosEmpresas")]
    public class VWServicosEmpresas
    {

        public int Id { get; set; }
        public string avaliacao { get; set; }
        public string servico { get; set; }
        public string valor { get; set; }
        public string motorista { get; set; }
        public string nomeFantasia { get; set; }
        public string telefone { get; set; }
        public string descricao { get; set; }
        public string endereco { get; set; }
        public string numero { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

        public string valorMini { get; set; }

        public string valorP{ get; set; }

        public string valorM{ get; set; }

        public string valorG { get; set; }
        public string foto { get; set; }
        public int formapagamento { get; set; }
        public double precokm { get; set; }

    }
}
