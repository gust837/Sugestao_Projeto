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

        public DbSet<SugestaoCategoria> SugestaoCategoria { get; set; }

        public DbSet<UsuarioVoto> UsuarioVoto { get; set; }

        public DbSet<Comentario> Comentario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SugestaoCategoria>().HasKey(sg => new {sg.CategoriaId, sg.SugestaoId});
            modelBuilder.Entity<UsuarioVoto>().HasKey(uv => new {uv.UsuarioId, uv.SugestaoId});
        }
        
    }
}