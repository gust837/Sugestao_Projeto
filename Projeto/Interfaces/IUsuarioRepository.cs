using Projeto.Models;

namespace Projeto.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> BuscarUsuario(string email, string senha);

        Task CriarUsuario(Usuario u);
    }
}