using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Projeto.Interfaces;
using Projeto.Models;

namespace Projeto.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly Sugestao_SenaiDbContext _context;

        public UsuarioRepository(Sugestao_SenaiDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> BuscarUsuario(string email, string senha)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);
        }

        public async Task CriarUsuario(Usuario u)
        {
            _context.Usuario.Add(u);
            await _context.SaveChangesAsync();
        }

        public async Task<int> Votar(int IdUsuario, int IdPost)
        {
            var voto = await _context.UsuarioVoto.FirstOrDefaultAsync(uv => uv.SugestaoId == IdPost && uv.UsuarioId == IdUsuario);
            var sug = await _context.Sugestao.FirstOrDefaultAsync(s => s.Id == IdPost);

            if (voto == null)
            {
                sug.Votos++;
            }
            else
            {
                sug.Votos--;
            }

            return sug.Votos;
        }
    }
}