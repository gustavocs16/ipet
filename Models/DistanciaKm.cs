using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class DistanciaKm
    {
        [Key]

        public string latitudeOrigem{ get; set; }
        public string longitudeOrigem { get; set; }
        public string latitudeDestino { get; set; }
        public string longitudeDestino { get; set; }

    }
}
