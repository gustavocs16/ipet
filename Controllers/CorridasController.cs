using API.DAO;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleMaps.LocationServices;
using System.Net;
using Flurl.Http;
using System.Security.Cryptography.X509Certificates;

namespace API.Controllers
{
    [Route("api/corridas")]
    [ApiController]
    public class CorridasController : ControllerBase
    {
        IConfiguration _configuration;
        public CorridasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return Ok("Retorno Ok");
        }


        [HttpPost]
        [Route("ServicosEmpresaValor")]
        public IActionResult GetServicosEmpresaValor(VWServicosEmpresas vw,
            [FromServices] ipetDAO ipetDAO)
        {

            try
            {
                var listaDeServicos = ipetDAO.ServicosEmpresasValor(vw);

                if (listaDeServicos != null)
                {
                    var resposta = JsonConvert.SerializeObject(listaDeServicos);
                    return Ok(resposta);
                }
                else
                {
                    return BadRequest("Nenhum serviço cadastrado");
                }
            }
            catch (Exception)
            {
                return BadRequest("Erro ao comunicar com a base de dados!");
            }
        }

        [HttpPost]
        [Route("ServicosEmpresaCidade")]
        public IActionResult GetServicosEmpresaCidade(VWServicosEmpresas vw,
    [FromServices] ipetDAO ipetDAO)
        {

            try
            {
                var listaDeServicos = ipetDAO.ServicosEmpresasCidade(vw);

                if (listaDeServicos != null)
                {
                    var resposta = JsonConvert.SerializeObject(listaDeServicos);
                    return Ok(resposta);
                }
                else
                {
                    return BadRequest("Nenhum serviço cadastrado");
                }
            }
            catch (Exception)
            {
                return BadRequest("Erro ao comunicar com a base de dados!");
            }
        }

        [HttpPost]
        [Route("ServicosEmpresaNome")]
        public IActionResult GetServicosEmpresaNome(VWServicosEmpresas vw,
[FromServices] ipetDAO ipetDAO)
        {

            try
            {
                var listaDeServicos = ipetDAO.ServicosEmpresasEstabelecimento(vw);

                if (listaDeServicos != null)
                {
                    var resposta = JsonConvert.SerializeObject(listaDeServicos);
                    return Ok(resposta);
                }
                else
                {
                    return BadRequest("Nenhum serviço cadastrado");
                }
            }
            catch (Exception)
            {
                return BadRequest("Erro ao comunicar com a base de dados!");
            }
        }


        [HttpPost]
        [Route("ServicosEmpresaAvaliacao")]
        public IActionResult GetServicosEmpresaAvaliacao(VWServicosEmpresas vw,
    [FromServices] ipetDAO ipetDAO)
        {

            try
            {
                var listaDeServicos = ipetDAO.ServicosEmpresasAvaliacao(vw);

                if (listaDeServicos != null)
                {
                    var resposta = JsonConvert.SerializeObject(listaDeServicos);
                    return Ok(resposta);
                }
                else
                {
                    return BadRequest("Nenhum serviço cadastrado");
                }
            }
            catch (Exception)
            {
                return BadRequest("Erro ao comunicar com a base de dados!");
            }
        }

        [HttpGet]
        [Route("CorridaParticular")]
        [Authorize(Roles = "manager,motorista")]
        public IActionResult GetCorridaParticular(
            [FromServices] ipetDAO ipetDAO)
        {

            try
            {
                var listaDeCorridas = ipetDAO.ListarParticular();

                if (listaDeCorridas != null)
                {
                    var resposta = JsonConvert.SerializeObject(listaDeCorridas);
                    return Ok(resposta);
                }
                else
                {
                    return BadRequest("Nenhuma corrida particular cadastrada!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Erro ao comunicar com a base de dados!");
            }
        }

        [HttpGet]
        [Route("CorridaConcluida")]
        public IActionResult GetCorridaConcluida(
    [FromServices] ipetDAO ipetDAO)
        {
   
            try
            {
                var listaDeCorridas = ipetDAO.ListarConcluida();

                if (listaDeCorridas != null)
                {
                    var resposta = JsonConvert.SerializeObject(listaDeCorridas);
                    return Ok(resposta);
                }
                else
                {
                    return BadRequest("Nenhuma corrida particular cadastrada!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Erro ao comunicar com a base de dados!");
            }
        }

        [HttpPost]
        [Route("cordenadas")]
        public async Task<dynamic> coordenadas(string end)
        {
            
            dynamic resultado;
            using (WebClient client = new WebClient())
            {
                var address = "rua rio itaqui 108,pinhais" + "&key=AIzaSyDvLB0-wwWF4Y97bf7E-nfI_ZJNE4fTJ6Y";
             
                var url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}", address);
                resultado = await url.GetJsonAsync();

                return resultado.results[0].geometry.location;
            }

        }


        [HttpPost]
        [Route("distanciakm")]
        public async Task<dynamic> distanciakm(DistanciaKm dk)
        {

            string latitudeOrigem = dk.latitudeOrigem;
            string latitudeDestino = dk.latitudeDestino;
            string longitudeOrigem = dk.longitudeOrigem;
            string longitudeDestino = dk.longitudeDestino;


            string dadosurl = latitudeOrigem + "," + longitudeOrigem + "&destinations=" + latitudeDestino + "," + longitudeDestino + "&key=AIzaSyDvLB0-wwWF4Y97bf7E-nfI_ZJNE4fTJ6Y";
            dynamic resultado;
            using (WebClient client = new WebClient())
            {

                var url = string.Format("https://maps.googleapis.com/maps/api/distancematrix/json?origins={0}", dadosurl);
                resultado = await url.GetJsonAsync();

                return resultado.rows[0].elements[0].distance.value ;
            }
        }

        [HttpPost]
        [Route("EncerraCorrida")]  
        public IActionResult EncerraCorrida([FromBody] int id,[FromServices] ipetDAO ipetDAO)
        {
                var encerra = ipetDAO.EncerraCorrida(id);             
                    var resposta = JsonConvert.SerializeObject(encerra);
                    return Ok(resposta);

        }


    }
}
