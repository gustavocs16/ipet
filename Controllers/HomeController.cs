using API.DAO;
using API.Models;
using API.Repositories;
using API.Services;
using Flurl.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PagarMe;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("v1/account")]
    public class HomeController : Controller
    {
        IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return Ok("Retorno Ok");
        }

        [HttpPost]
        [Route("Adicionar")]
        public IActionResult CriaUsuario([FromForm] User user)
        {
            if (user == null)
            {
                return BadRequest("Informações invalidas");
            }
            else
            {
                var cpf = new ipetDAO(_configuration).ValidaCPF(user);
                if(cpf > 0)
                {
                    return BadRequest("CPF JÁ CADASTRADO");
                }
                else
                {
                    if(user.Image == null)
                    {
                        user.foto_perfil = "http://ipet.kinghost.net/imagens/pngwing.com.png";
                    }
                    else
                    {
                        // Getting Image
                        var image = user.Image;
                        // Saving Image on 

                        var filePath = Path.Combine("./Imagens/", image.FileName);
                        if (image.Length > 0)
                        {
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                image.CopyTo(fileStream);
                            }
                        }

                        user.foto_perfil = "http://ipet.kinghost.net/imagens/" + image.FileName;
                    }
                    var cadastrar = new ipetDAO(_configuration).Adicionar(user);
                    return Ok(cadastrar);
                }
                
       
            }

        }


        [HttpPost]
        [Route("AdicionarPetCompartilhado")]
        public IActionResult CriaPetCompartilhado([FromForm] Pet pet)
        {
            if (pet == null)
            {
                return BadRequest("Informações invalidas");
            }
            else
            {
                var key = new ipetDAO(_configuration).ValidaCode(pet);
                if (key > 0)
                {
                    var cadastrar = new ipetDAO(_configuration).AdicionarPetCompartilhado(pet);
                    return Ok(cadastrar);                    
                }
                else
                {
                    return BadRequest(key);
                }
            }

        }

        [HttpPost]
        [Route("AdicionarPet")]
        public IActionResult CriaPet([FromForm] Pet pet)
        {
            if (pet == null)
            {
                return BadRequest("Informações invalidas");
            }
            else
            {
                    if (pet.Image == null)
                    {
                       pet.foto = "http://ipet.kinghost.net/imagens/pngwing.com.png";
                    }
                    else
                    {
                        // Getting Image
                        var image = pet.Image;
                        // Saving Image on 

                        var filePath = Path.Combine("./Imagens/", image.FileName);
                        if (image.Length > 0)
                        {
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                image.CopyTo(fileStream);
                            }
                        }

                        pet.foto = "http://ipet.kinghost.net/imagens/" + image.FileName;
                    }
                    var cadastrar = new ipetDAO(_configuration).AdicionarPet(pet);
                    return Ok(cadastrar);
                


            }

        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Log([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Informações invalidas");
            }

            var cadastra = new ipetDAO(_configuration).Login(user);
            var token = TokenService.GenerateToken(cadastra);
            //cadastra.Password = "";
            return new
            {
                user = cadastra,
                token = token
            };



        }
        [HttpPost]
        [Route("DadosPessoais")]
        public IActionResult DadosPessoais(DadosPessoais usr,
[FromServices] ipetDAO ipetDAO)
        {

            try
            {
                var Dados = ipetDAO.DadosPessoais(usr);

                if (Dados != null)
                {
                    var resposta = JsonConvert.SerializeObject(Dados);
                    return Ok(resposta);
                }
                else
                {
                    return BadRequest("Nenhum dado encontrado");
                }
            }
            catch (Exception)
            {
                return BadRequest("Erro ao comunicar com a base de dados!");
            }
        }

        [HttpPost]
        [Route("Deletar")]
        public IActionResult DeletaUsuario([FromBody] string user)
        {
            if (user == null)
            {
                return BadRequest("informações invalidas");
            }
            else
            {
                var deletar = new ipetDAO(_configuration).Delete(user);
                return Ok(deletar);
            }

        }


        [HttpPost]
        [Route("ValidaCPF")]
        public IActionResult ValidaCPF([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("informações invalidas");
            }
            else
            {
               
                var CPF = new ipetDAO(_configuration).ValidaCPF(user);
                return Ok(CPF);
            }

        }

        [HttpPost]
        [Route("RetornaDados")]
        public IActionResult RetornaDados([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("informações invalidas");
            }
            else
            {

                var dados = new ipetDAO(_configuration).dadosUsuarios(user);
                return Ok(dados);
            }

        }

        [HttpPost]
        [Route("RetornaDadosCPF")]
        public IActionResult RetornaDadosCPF([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("informações invalidas");
            }
            else
            {

                var dados = new ipetDAO(_configuration).dadosUsuariosCPF(user);
                return Ok(dados);
            }

        }

        [HttpPost]
        [Route("loginx")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
        {
            var user = UserRepository.Get(model.Email, model.Password);

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);
            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }
        public static bool IsPropertyExist(dynamic settings, string name)
        {
            if (settings is ExpandoObject)
                return ((IDictionary<string, object>)settings).ContainsKey(name);

            return settings.GetType().GetProperty(name) != null;
        }

        [HttpPost]
        [Route("AdicionarEmpresa")]
        public async Task<IActionResult> AdicionarempresaAsync([FromBody] Empresa empresa)
        {
            if (empresa == null)
            {
                return BadRequest("Informações invalidas");
            }
            else
            {
                dynamic resultado;

                using (WebClient client = new WebClient())
                {
                    string result = "";
                    var url = string.Format("https://www.receitaws.com.br/v1/cnpj/{0}", empresa.cnpj);
                    resultado = await url.GetJsonAsync();

                    if (IsPropertyExist(resultado, "message"))
                    {
                        return BadRequest("CNPJ inválido");
                    }
                    else
                    {
                        result = resultado.situacao;
                        if (result == "ATIVA")
                        {
                            var cadastrar = new ipetDAO(_configuration).AdicionarEmpresa(empresa);
                            return Ok(cadastrar);
                        }
                        else
                        {
                            return BadRequest("cnpj inativo");
                        }
                    }


                }

            }

        }

        [HttpPost]
        [Route("AlterarUsuario")]
        public async Task<IActionResult> AlterarUsuario([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Informações invalidas");
            }
            else
            {
                            var altera = new ipetDAO(_configuration).Edit(user);
                            return Ok(altera);
               
            }

        }
        [HttpPost]
        [Route("Pagarme")]
        public async Task<IActionResult> pagar()
        {
            PagarMeService.DefaultEncryptionKey = "ek_test_NA3xJ9GOfZQylBmi0ifhbOE6rOfUkm";

            CardHash card = new CardHash
            {
                CardNumber = "4111111111111111",
                CardHolderName = "Morpheus Fishburne",
                CardExpirationDate = "0922",
                CardCvv = "123"
            };

            string cardhash = card.Generate();

            return Ok(cardhash);
      
        }



        [HttpPost]
        [Route("Encode64")]
        public async Task<IActionResult> Base64Encode([FromBody] Pet pet)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(pet.nome + pet.Id);
            return Ok(System.Convert.ToBase64String(plainTextBytes));
        }

        [HttpPost]
        [Route("Decode64")]
        public async Task<IActionResult> Base64Decode([FromBody] Pet pet)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(pet.nome);
            return Ok(System.Text.Encoding.UTF8.GetString(base64EncodedBytes));
        }


        [HttpPost]
        [Route("AdicionarEndereco")]
        public async Task<IActionResult> Adicionarendereco([FromBody] End endereco)
        {

            string address = endereco.endereco + ", " + endereco.numero + "&key=AIzaSyDvLB0-wwWF4Y97bf7E-nfI_ZJNE4fTJ6Y";
            //return Ok(adress);
            if (endereco == null)
            {
                return BadRequest("INFORMAÇÕES INVALIDAS");
            }
            else
            {
                dynamic resultado;
                dynamic lat;
                dynamic lng;
                dynamic bairro;
                dynamic cidade;
                using (WebClient client = new WebClient())
                {
                    //var address = "Rua Rio itaqui 108, pinhais" + "&key=AIzaSyDvLB0-wwWF4Y97bf7E-nfI_ZJNE4fTJ6Y";
                    var url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}", address);
                    resultado = await url.GetJsonAsync();
                    lat = resultado.results[0].geometry.location.lat;
                    lng = resultado.results[0].geometry.location.lng;
                    bairro = resultado.results[0].address_components[2].long_name;
                    cidade = resultado.results[0].address_components[3].long_name;

                    endereco.bairro = bairro;
                    endereco.latitude = Convert.ToString(lat).Replace(',', '.'); ;
                    endereco.longitude = Convert.ToString(lng).Replace(',', '.'); ;
                    endereco.cidade = cidade;


                    var cadastrar = new ipetDAO(_configuration).AdicionarEndereco(endereco);
                    return Ok(cadastrar);
                }


                
            }
        }

        [HttpPost]
        [Route("EditarEndereco")]
        public async Task<IActionResult> EditarEndereco([FromBody] End endereco)
        {

            string address = endereco.endereco + ", " + endereco.numero + ", " + endereco.cidade + "&key=AIzaSyDvLB0-wwWF4Y97bf7E-nfI_ZJNE4fTJ6Y";
            //return Ok(adress);
            if (endereco == null)
            {
                return BadRequest("INFORMAÇÕES INVALIDAS");
            }
            else
            {
                dynamic resultado;
                dynamic lat;
                dynamic lng;
                dynamic bairro;
                dynamic cidade;
                using (WebClient client = new WebClient())
                {
                    //var address = "Rua Rio itaqui 108, pinhais" + "&key=AIzaSyDvLB0-wwWF4Y97bf7E-nfI_ZJNE4fTJ6Y";
                    var url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}", address);
                    resultado = await url.GetJsonAsync();
                    lat = resultado.results[0].geometry.location.lat;
                    lng = resultado.results[0].geometry.location.lng;
                    bairro = resultado.results[0].address_components[2].long_name;
                    cidade = resultado.results[0].address_components[3].long_name;

                    endereco.bairro = bairro;
                    endereco.latitude = Convert.ToString(lat).Replace(',', '.'); ;
                    endereco.longitude = Convert.ToString(lng).Replace(',', '.'); ;
                    endereco.cidade = cidade;


                    var alterar= new ipetDAO(_configuration).AlterarEndereco(endereco);
                    return Ok(alterar);
                }



            }
        }

            [HttpPost]
            [Route("DeletaEndereco")]
            public async Task<IActionResult> DeletaEndereco([FromBody] End endereco)
            {

                if (endereco == null)
                {
                    return BadRequest("INFORMAÇÕES INVALIDAS");
                }
                else
                {      
                        var deletar = new ipetDAO(_configuration).DeletaEndereco(endereco);
                        return Ok(deletar);
                    }



             }

            [HttpPost]
        [Route("PegaEndereco")]
        public IActionResult GetEndereco([FromBody] End end,
    [FromServices] ipetDAO ipetDAO)
        {

            try
            {
                var listaDeEndereco = ipetDAO.Endereco(end);

                if (listaDeEndereco != null)
                {
                    var resposta = JsonConvert.SerializeObject(listaDeEndereco);
                    return Ok(resposta);
                }
                else
                {
                    return BadRequest("Nenhum endereço cadastrado");
                }
            }
            catch (Exception)
            {
                return BadRequest("Erro ao comunicar com a base de dados!");
            }
        }


        [HttpPost]
        [Route("PegaPet")]
        public IActionResult GetPet([FromBody] Pet pet,
[FromServices] ipetDAO ipetDAO)
        {

            try
            {
                var listaDePet = ipetDAO.Pet(pet);

                if (listaDePet != null)
                {
                    var resposta = JsonConvert.SerializeObject(listaDePet);
                    return Ok(resposta);
                }
                else
                {
                    return BadRequest("Nenhum pet cadastrado");
                }
            }
            catch (Exception)
            {
                return BadRequest("Erro ao comunicar com a base de dados!");
            }
        }

        [HttpPost]
        [Route("UploadFile")]
        public ActionResult Student([FromForm] User std)
        {
            // Getting Name
            string name = std.Nome;
            // Getting Image
            var image = std.Image;
            // Saving Image on 

            var filePath = Path.Combine("./Imagens/", image.FileName);
            if (image.Length > 0)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            return Ok(new { status = true, message = "Imagem postada com sucesso." + filePath });
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => String.Format("Autenticado");

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "employee,manager")]
        public string Employee() => "Funcionário";

        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "manager")]
        public string Manager() => "Gerente";

    }
}
