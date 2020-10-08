using API.Models;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Flurl.Http;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace API.DAO
{
    public class ipetDAO
    {
        private IConfiguration _configuracoes;

        public ipetDAO(IConfiguration config)
        {
            _configuracoes = config;
        }


        public async Task<ActionResult<dynamic>> validacnpj([FromBody] Empresa empresa)
        {

            dynamic resultado;
            using (WebClient client = new WebClient())
            {
                var url = string.Format("https://www.receitaws.com.br/v1/cnpj/{0}", empresa.cnpj);
                resultado = await url.GetJsonAsync();

                return resultado.situacao;
            }

        }

        public async Task<dynamic> coordenadas(string end)
        {

            dynamic resultado;
            using (WebClient client = new WebClient())
            {
                var address = "Rua Rio itaqui 108, pinhais" + "&key=AIzaSyDvLB0-wwWF4Y97bf7E-nfI_ZJNE4fTJ6Y";
                var url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}", address);
                resultado = await url.GetJsonAsync();

                return resultado.results[0].geometry.location;
            }

        }

        public async Task<dynamic> DistanciaemKm([FromBody] DistanciaKm distancia)
        {
            string latitudeOrigem = distancia.latitudeOrigem;
            string latitudeDestino = distancia.latitudeDestino;
            string longitudeOrigem = distancia.longitudeOrigem;
            string longitudeDestino = distancia.longitudeDestino;


            string dadosurl = latitudeOrigem + "," + longitudeOrigem +  "&destinations=" + latitudeDestino + "," + longitudeDestino + "&key=AIzaSyDvLB0-wwWF4Y97bf7E-nfI_ZJNE4fTJ6Y";
            dynamic resultado;
            using (WebClient client = new WebClient())
            {
                
                var url = string.Format("https://maps.googleapis.com/maps/api/distancematrix/json?origins={0}", dadosurl);
                resultado = await url.GetJsonAsync();

                return resultado.results[0];
            }

        }


        public IEnumerable<CorridaCompartilhada> ListarCompartilhada()
        {
            using (MySqlConnection conexao = new MySqlConnection(
                _configuracoes.GetConnectionString("iPet")))
            {

                return conexao.GetAll<CorridaCompartilhada>();
            }
        }

        public List<End> Endereco(End end)
        {
            using (MySqlConnection con = new MySqlConnection(
                _configuracoes.GetConnectionString("iPet")))
            {
                var query = @" SELECT * FROM Endereco where id_Pessoa = @id and status = 0; ";
                var endereco = con.Query<End>(query, new { id = end.Id });
                return endereco.ToList();


            }
        }

        public List<Pet> Pet(Pet pet)
        {
            using (MySqlConnection con = new MySqlConnection(
                _configuracoes.GetConnectionString("iPet")))
            {
                var query = @" SELECT * FROM Pet where id_Pessoa = @id; ";
                var listpet = con.Query<Pet>(query, new { id = pet.Id });
                return listpet.ToList();


            }
        }

        public List<VWServicosEmpresas> ServicosEmpresasValor(VWServicosEmpresas vw)
        {
            using (MySqlConnection con = new MySqlConnection(
                _configuracoes.GetConnectionString("iPet")))
            {
                var query = @" SELECT * FROM VWServicosEmpresas where cidade = @cidade ORDER BY VALOR ASC; ";
                var products = con.Query<VWServicosEmpresas>(query, new { id = vw.Id, cidade = vw.cidade });
                return products.ToList();


            }
        }

        public List<VWServicosEmpresas> ServicosEmpresasEstabelecimento(VWServicosEmpresas vw)
        {
            using (MySqlConnection con = new MySqlConnection(
                _configuracoes.GetConnectionString("iPet")))
            {
                var query = @" SELECT * FROM VWServicosEmpresas where nomeFantasia Like @nomeFantasia and cidade = @cidade";
                var products = con.Query<VWServicosEmpresas>(query, new { cidade = vw.cidade, nomeFantasia = "%" + vw.nomeFantasia + "%"});
                return products.ToList();


            }
        }

        public List<VWServicosEmpresas> ServicosEmpresasCidade(VWServicosEmpresas vw)
        {
            using (MySqlConnection con = new MySqlConnection(
                _configuracoes.GetConnectionString("iPet")))
            {
                    var query = @" SELECT * FROM VWServicosEmpresas WHERE cidade = @cidade; ";
                    var products = con.Query<VWServicosEmpresas>(query, new { cidade = vw.cidade });
                    return products.ToList();
               
            }
        }

        public List<DadosPessoais> DadosPessoais(DadosPessoais usr)
        {
            using (MySqlConnection con = new MySqlConnection(
                _configuracoes.GetConnectionString("iPet")))
            {
                var query = @" SELECT * FROM VWPessoa_Dado WHERE id = @id; ";
                var dados = con.Query<DadosPessoais>(query, new { id = usr.id });
                return dados.ToList();
            }
        }

        public List<VWServicosEmpresas> ServicosEmpresasAvaliacao(VWServicosEmpresas vw)
        {

            using (MySqlConnection con = new MySqlConnection(
                _configuracoes.GetConnectionString("iPet")))
            {
                    var query = @" SELECT * FROM VWServicosEmpresas where cidade = @cidade ORDER BY AVALIACAO DESC; ";
                    var products = con.Query<VWServicosEmpresas>(query, new { cidade = vw.cidade });
                    return products.ToList();            
            }
        }




        public IEnumerable<CorridaParticular> ListarParticular()
        {
            using (MySqlConnection conexao = new MySqlConnection(
                _configuracoes.GetConnectionString("iPet")))
            {

                return conexao.GetAll<CorridaParticular>();
            }
        }


        public IEnumerable<CorridaConcluida> ListarConcluida()
        {
            using (MySqlConnection conexao = new MySqlConnection(
                _configuracoes.GetConnectionString("iPet")))
            {

                return conexao.GetAll<CorridaConcluida>();
            }
        }

        public string GetConnection()
        {
            var connection = _configuracoes.GetSection("ConnectionStrings").GetSection("iPet").Value;
            return connection;
        }
        public User Login(User user)
        {
            string mail = user.Email.ToLower();
            string pass = user.Password;

            var connectionString = this.GetConnection();
            string resultado = "";
            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    User users = new User();
                    con.Open();
                    var query = "call SP_LoginUsuario(@email,@password)";
                    resultado = con.QueryFirstOrDefault<String>(query, new { email = mail, password = pass });
                    JObject json = JObject.Parse(resultado);
                    JToken token = JObject.Parse(resultado);
                    //Console.Write(resultado);
                    users.Id = (int)token.SelectToken("Id");
                    users.Nome = (string)token.SelectToken("Nome");
                    users.Email = (string)token.SelectToken("Email");
                    users.Password = (string)token.SelectToken("Password");
                    users.foto_perfil = (string)token.SelectToken("Foto_perfil");
                    users.Role = (string)token.SelectToken("Role");
                    Console.WriteLine(users);
                    return users;



                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                //return resultado;
            }
        }
        public dynamic ValidaCPF(User user)
        {
            var connectionString = this.GetConnection();
            
            
            using (var con = new MySqlConnection(connectionString))
            {
                var query = "SELECT COUNT(*) FROM `Pessoa` WHERE cpf = @cpf";
                var quantidade = con.QueryFirstOrDefault<int>(query, new { cpf = user.Cpf });

                return quantidade;

            }
        }

        public dynamic ValidaCode(Pet pet)
        {
            var connectionString = this.GetConnection();


            using (var con = new MySqlConnection(connectionString))
            {
                var query = "SELECT COUNT(*) FROM `Pet` WHERE cod = 'VGhlbzE='";
                var quantidade = con.QueryFirstOrDefault<int>(query);

                return quantidade;

            }
        }

        public dynamic dadosUsuarios(User user)
        {
            var connectionString = this.GetConnection();
           

            using (var con = new MySqlConnection(connectionString))
            {
                var query = "select json_object('nome', nome, 'cpf', cpf, 'data_nascimento', data_nascimento, 'email', email, 'telefone', telefone) FROM `Pessoa` WHERE id = @id";
                var resultado = con.QueryFirstOrDefault<string>(query, new { id = user.Id });

                return resultado;

            }
        }


        public dynamic dadosUsuariosCPF(User user)
        {
            var connectionString = this.GetConnection();


            using (var con = new MySqlConnection(connectionString))
            {
                var query = "select json_object('id',id,'nome', nome, 'data_nascimento', data_nascimento, 'telefone', telefone) FROM `Pessoa` WHERE cpf = @cpf";
                var resultado = con.QueryFirstOrDefault<string>(query, new { cpf = user.Cpf });

                return resultado;

            }
        }



        public string Adicionar(User user)
        {
            var connectionString = this.GetConnection();
            int count = 0;
            string ok = "ok";
            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "INSERT INTO Pessoa(nome, cpf, Data_nascimento, email, telefone, tipo_usuario, senha,role, foto_perfil) VALUES (@nome, @cpf, @data_nascimento, @email, @telefone, @tipo_usuario, @senha, @role, @foto_perfil)";
                    count = con.Execute(query, new { nome = user.Nome, cpf = user.Cpf, Data_nascimento = user.data_nascimento, email = user.Email, telefone = user.Telefone, tipo_usuario = user.Tipo_usuario, senha = user.Password, role = user.Role, foto_perfil = user.foto_perfil });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                return ok;
            }
        }


        public string AdicionarPet(Pet pet)
        {
            var connectionString = this.GetConnection();
            int count = 0;
            string ok = "ok";
            string porte = "";
            if(pet.peso > 0 && pet.peso <= 6 )
            {
                porte = "Mini";
            }
            if(pet.peso > 6 && pet.peso <= 15)
            {
                porte = "P";
            } 
            if(pet.peso > 15 && pet.peso <= 25)
            {
                porte = "G";
            }
            if(pet.peso > 25)
            {
                porte = "G";
            }



            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "INSERT INTO Pet(nome, raca, foto, id_pessoa, porte, peso, data_nascimento) VALUES (@nome, @raca, @foto, @id_pessoa, @porte, @peso, @data_nascimento)";
                    count = con.Execute(query, new { nome = pet.nome, raca = pet.raca, foto = pet.foto, id_pessoa = pet.id_pessoa, porte = porte, peso = pet.peso, data_nascimento = pet.data_nascimento});
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                return ok;
            }
        }

        public string AdicionarPetCompartilhado(Pet pet)
        {
            var connectionString = this.GetConnection();
            int count = 0;
            string ok = "ok";

            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "INSERT INTO Pet_Cliente(id_pet, id_pessoa) VALUES (@id_pet, @id_pessoa)";
                    count = con.Execute(query, new { id_pet = pet.Id, id_pessoa = pet.id_pessoa});
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                return ok;
            }
        }

        public string Delete(string user)
        {

            var connectionString = this.GetConnection();
            var count = 0;
            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "DELETE FROM Pessoa WHERE id=@id";
                    count = con.Execute(query, new { id = user });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                return user;
            }
        }

        public int Edit(User user)
        {
            var connectionString = this.GetConnection();
            var count = 0;
            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "UPDATE Pessoa SET nome = @nome, data_nascimento = @data_nascimento, telefone = @telefone WHERE id = @id";
                    count = con.Execute(query, new { nome = user.Nome, data_nascimento = user.data_nascimento, telefone = user.Telefone, id = user.Id });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                return count;
            }
        }


        public async Task<string> AdicionarEmpresa(Empresa empresa)
        {
            var connectionString = this.GetConnection();
            int count = 0;
            string cnpj = empresa.cnpj;
            if (empresa.cnpj != null)
            {
                using (var con = new MySqlConnection(connectionString))
                {
                    try
                    {
                        con.Open();
                        var query = "INSERT INTO Empresa(nomeFantasia, cnpj, razaoSocial, email, telefone, responsavel) VALUES (@nomeFantasia, @cnpj, @razaoSocial, @email, @telefone, @responsavel)";
                        count = con.Execute(query, new { nomeFantasia = empresa.nomefantasia, cnpj = empresa.cnpj, razaoSocial = empresa.razaosocial, email = empresa.email, telefone = empresa.telefone, responsavel = empresa.responsavel });
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }
                    return "cadastro efetuado";
                }
            }
            else
            {
                return "teste";
            }




        }

        public string AdicionarServico(Servico servico)
        {
            var connectionString = this.GetConnection();
            int count = 0;
            string ok = "ok";
            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "INSERT INTO Servico(nome, descricao) VALUES (@nome, @descricao)";
                    count = con.Execute(query, new { nome = servico.Nome, descricao = servico.Descricao });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                return ok;
            }
        }

        public dynamic AdicionarEndereco(End endereco)
        {
            var connectionString = this.GetConnection();
            int count = 0;
            



            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "INSERT INTO Endereco(endereco,numero,cep,longitude,latitude,complemento,id_pessoa,id_empresa,bairro,cidade) VALUES" +
                        "(@endereco,@numero,@cep,@longitude,@latitude,@complemento,@id_pessoa,@id_empresa,@bairro,@cidade)";
                    count = con.Execute(query, new { endereco = endereco.endereco, numero = endereco.numero, cep = endereco.cep, longitude = endereco.longitude, latitude = endereco.latitude, complemento = endereco.complemento, id_pessoa = endereco.id_pessoa, id_empresa = endereco.id_empresa, bairro = endereco.bairro, cidade = endereco.cidade });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                return "endereço cadastrado:" + endereco.endereco + ", " + endereco.numero + "," + endereco.bairro + " " + endereco.cidade;
            }
        }


        public dynamic AlterarEndereco(End endereco)
        {
            var connectionString = this.GetConnection();
            int count = 0;




            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "UPDATE `Endereco` set endereco = @endereco , numero = @numero, cep = @cep,longitude = @longitude, latitude = @latitude, complemento = @complemento, id_pessoa = @id_pessoa, id_empresa = @id_empresa, bairro = @bairro, cidade = @cidade where id = @id";
                       
                    count = con.Execute(query, new { id = endereco.Id, endereco = endereco.endereco, numero = endereco.numero, cep = endereco.cep, longitude = endereco.longitude, latitude = endereco.latitude, complemento = endereco.complemento, id_pessoa = endereco.id_pessoa, id_empresa = endereco.id_empresa, bairro = endereco.bairro, cidade = endereco.cidade });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                return "endereço cadastrado:" + endereco.endereco + ", " + endereco.numero + "," + endereco.bairro + " " + endereco.cidade;
            }
        }

        public dynamic DeletaEndereco(End endereco)
        {
            var connectionString = this.GetConnection();
            int count = 0;




            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "UPDATE `Endereco` SET status = 1 WHERE id = @id";
                    count = con.Execute(query, new { id = endereco.Id});
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                return "endereço deletado";
            }
        }

        public int EncerraCorrida(int id)
        {
            var connectionString = this.GetConnection();
            var count = 0;
            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "UPDATE Corrida_Motorista SET status_corrida_cod = 2 where id =" + id;
                    count = con.Execute(query);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                return id;
            }
        }
    }
}
