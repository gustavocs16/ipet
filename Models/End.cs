using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class End
    {

        [Key]
        public int Id { get; set; }
        public string endereco { get; set; }
        public string numero { get; set; }
        public string cep { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string complemento { get; set; }
        public int id_pessoa { get; set; }
        public int id_empresa { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }





    }
}
