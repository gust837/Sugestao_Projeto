using Projeto.Models;

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