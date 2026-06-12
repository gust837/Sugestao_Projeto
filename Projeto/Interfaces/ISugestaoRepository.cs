using Projeto.Models;

namespace Projeto.Interfaces
{
    public interface ISugestaoRepository
    {
        Task CriarSugestao(Sugestao s);

        Task CriarSugCat(Sugestao_Categoria sc);

        Task EditarSugestao(Sugestao s);

        Task ExcluirSugestao(Sugestao s);

        Task ExcluirSugCat(int sugId);

        Task<IEnumerable<Sugestao>> ListarSugestoes();

        Task<IEnumerable<Sugestao>> ListarSugestaoPorStatus(string status);

        Task<IEnumerable<Sugestao>> ListarSugestaoPorCategoria(int CategoriaId);

        Task<bool> VerificarUsuarioVoto(int usuarioId, int postId);

        Task<Sugestao?> ProcurarSugestao(int sugId);

        Task Votar(Sugestao s, Usuario_Voto uv);

        Task RemoverVoto(Sugestao s, Usuario_Voto uv);
    }
}