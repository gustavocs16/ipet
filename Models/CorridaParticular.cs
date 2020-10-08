using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("VWCorridas_Motorista_Particular")]
    public partial class CorridaParticular

    {
        public int Id { get; set; }
        public string cliente { get; set; }
        public string motorista { get; set; }
        public string endereco { get; set; }
        public int numero { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string data_corrida { get; set; }
        public string status_corrida { get; set; }
        public string tipo_corrida { get; set; }
        public string controleValor { get; set; }
        public string pet { get; set; }
        public string foto_pet { get; set; }



    }
}
