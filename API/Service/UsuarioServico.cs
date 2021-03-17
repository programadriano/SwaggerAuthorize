using API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Service
{
    public class UsuarioServico : IUsuarioServico
    {
        public bool ValidaCredenciais(string usuario, string senha)
        {
            return usuario.Equals("batman") && senha.Equals("batman");
        }
    }
}
