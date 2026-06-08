using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projeto.Model;

namespace Projeto.Interfaces
{
    public interface ISugestaoRepositoty
    {
        Task CriarSugestao(Sugestao s);

        Task EditarStatusSugestao();

        Task ExcluirSugestao();

        void Votar(int UsuarioId, int PostId);

        Task<IEnumerable<Sugestao>> ListarSugestoes();

        Task<IEnumerable<Sugestao>> ListarSugestaoPorStatus(string status);
    }
}