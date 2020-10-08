using API.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        IConfiguration _configuration;
        public UsuarioRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").GetSection("iPet").Value;
            return connection;
        }

        public int Add(User user)
        {
            var connectionString = this.GetConnection();
            int count = 0;
            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "INSERT INTO Pessoa(nome, cpf, data_nascimento, email, telefone, tipo_usuario, senha) VALUES (@nome, @cpf, @data_nascimento, @email, @telefone, @tipo_usuario, @senha)";
                    count = con.Execute(query, user);
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



        public int Delete(int id)
        {
            var connectionString = this.GetConnection();
            var count = 0;
            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "DELETE FROM Usuarios WHERE id=" + id;
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
                return count;
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
                    var query = "UPDATE Produtos SET Name = @Nome, Estoque = @Estoque, Preco = @Preco WHERE ProdutoId = " + user.Id;
                    count = con.Execute(query, user);
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


    }

    
}
