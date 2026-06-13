using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Projeto.Interfaces;
using Projeto.Models;

namespace Projeto.Repositories
{
    public class SugestaoRepository : ISugestaoRepository
    {
        private readonly Sugestao_SenaiDbContext _context;

        public SugestaoRepository(Sugestao_SenaiDbContext context)
        {
            _context = context;
        }

        public async Task CriarSugCat(Sugestao_Categoria sc)
        {
            await _context.Sugestao_Categoria.AddAsync(sc);
            await _context.SaveChangesAsync();
        }

        public async Task CriarSugestao(Sugestao s)
        {
            await _context.Sugestao.AddAsync(s);
            await _context.SaveChangesAsync();
        }

        public async Task EditarSugestao(Sugestao s)
        {
            _context.Sugestao.Update(s);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirSugestao(Sugestao s)
        {
            _context.Sugestao.Remove(s);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Sugestao>> ListarSugestaoPorCategoria(int categoriaId)
        {
            return await _context.Sugestao.Where(s => s.Categorias.Any(c => c.Id == categoriaId)).OrderByDescending(r => r.Votos).ToListAsync();
        }

        public async Task<IEnumerable<Sugestao>> ListarSugestaoPorStatus(string status)
        {
            return await _context.Sugestao.Where(s => s.StatusSugestao == status).OrderByDescending(r => r.Votos).ToListAsync();
        }

        public async Task<IEnumerable<Sugestao>> ListarSugestoes()
        {
            return await _context.Sugestao.OrderByDescending(r => r.Votos).ToListAsync();
        }

        public async Task<bool> VerificarUsuarioVoto(int usuarioId, int postId)
        {
            return await _context.Usuario_Voto.FirstOrDefaultAsync(uv => uv.UsuarioId == usuarioId && uv.SugestaoId == postId) != null;
        }

        public async Task<Sugestao?> ProcurarSugestao(int sugId)
        {
            return await _context.Sugestao.FirstOrDefaultAsync(s => s.Id == sugId);
        }

        public async Task Votar(Sugestao s, Usuario_Voto uv)
        {
            _context.Sugestao.Update(s);
            _context.Usuario_Voto.Add(uv);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverVoto(Sugestao s, int usuarioId, int sugestaoId)
        {
            _context.Sugestao.Update(s);
            _context.Sugestao.Update(s);

            var uvExistente = await _context.Usuario_Voto
                .FindAsync(usuarioId, sugestaoId);
            if (uvExistente != null)
                _context.Usuario_Voto.Remove(uvExistente);

            await _context.SaveChangesAsync();
        }

        public async Task ExcluirSugCat(int sugId)
        {
            IEnumerable<Sugestao_Categoria> sgs = _context.Sugestao_Categoria.Where(sg => sg.SugestaoId == sugId);
            _context.Sugestao_Categoria.RemoveRange(sgs);
            await _context.SaveChangesAsync();
        }
    }
}