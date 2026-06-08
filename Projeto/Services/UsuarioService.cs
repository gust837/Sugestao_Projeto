using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projeto.Interfaces;
using Projeto.Model;

namespace Projeto.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<Usuario?> BuscarUsuario(string email, string senha)
        {
            return await _repository.BuscarUsuario(email, senha);
        }

        public async Task CriarUsuario(Usuario u)
        {
            await _repository.CriarUsuario(u);
        }

        public async Task<int> Votar(int UsuarioId, int PostId)
        {
            return await _repository.Votar(UsuarioId, PostId);
        }
    }
}