using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Projeto.Models;

namespace Projeto
{
    public class Sugestao_SenaiDbContext : DbContext
    {
        public Sugestao_SenaiDbContext(DbContextOptions<Sugestao_SenaiDbContext> options) : base(options) {}

        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<Sugestao> Sugestao { get; set; }

        public DbSet<Categoria> Categoria { get; set; }

        public DbSet<Sugestao_Categoria> Sugestao_Categoria { get; set; }

        public DbSet<Usuario_Voto> Usuario_Voto { get; set; }

        public DbSet<Comentario> Comentario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sugestao_Categoria>().HasKey(sg => new {sg.CategoriaId, sg.SugestaoId});
            modelBuilder.Entity<Usuario_Voto>().HasKey(uv => new {uv.UsuarioId, uv.SugestaoId});
        }
        
    }
}