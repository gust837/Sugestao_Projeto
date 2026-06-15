using Projeto.Models;

namespace Projeto.Interfaces
{
    public interface IUsuarioRepository
    {
        Task CriarUsuario(Usuario u);

        Task<Usuario?> BuscarUsuarioEmailSenha(string email, string senha);

        Task<bool> BuscarUsuario(Usuario u);
    }
}