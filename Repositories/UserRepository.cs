using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class UserRepository
    {
        public static User Get(string username, string password)
        {
            var users = new List<User>
            {
                new User { Id = 1, Email = "batman", Password = "batman", Role = "manager" },
                new User { Id = 2, Email = "robin", Password = "robin", Role = "employee" }
            };
            return users.Where(x => x.Email.ToLower() == username.ToLower() && x.Password == x.Password).FirstOrDefault();
        }
    }
}
