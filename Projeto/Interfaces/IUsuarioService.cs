using Projeto.Models;

namespace Projeto.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario?> BuscarUsuario(string email, string senha);

        Task CriarUsuario(Usuario u);
    }
}