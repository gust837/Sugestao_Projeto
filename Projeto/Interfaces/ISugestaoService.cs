
using Projeto.Models;

namespace Projeto.Interfaces
{
    public interface ISugestaoService
    {
        Task CriarSugestao(Sugestao s, string? categorias, IFormFile arquivoImagem);

        Task ExcluirSugestao(int sugId);

        Task Votar(int UsuarioId, int PostId);

        Task EditarStatusSugestao(int sugId, string status);

        Task<IEnumerable<Sugestao>> ListarSugestoes();

        Task<IEnumerable<Sugestao>> ListarSugestaoPorStatus(string status);

        Task<IEnumerable<Sugestao>> ListarSugestaoPorCategoria(int CategoriaId);
    }
}