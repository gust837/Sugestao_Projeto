using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Interfaces
{
    public interface ISugestaoService
    {
        void Votar(int UsuarioId, int PostId);
    }
}