using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domain
{
    public interface IUsuarioServico
    {
        bool ValidaCredenciais(string usuario, string senha);
    }
}
