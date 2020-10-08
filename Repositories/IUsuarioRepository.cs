using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public interface IUsuarioRepository
    {
        int Add(User user);
        int Edit(User user);
        int Delete(int id);
    }
}
