using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Pet
    {

        [Key]
        public int Id { get; set; }
        public string nome { get; set; }
        public string raca { get; set; }
        public string data_nascimento { get; set; }
        public string foto { get; set; }
        public int id_pessoa { get; set; }
        public string porte { get; set; }

        public string valorMini { get; set; }
        public string valorP { get; set; }

        public string valorM { get; set; }

        public string valorG { get; set; }
        public float peso { get; set; }
        public string cod { get; set; }

        public IFormFile Image { get; set; }




    }
}
