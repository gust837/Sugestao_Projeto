using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Model
{
    public class Sugestao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = null!;

        public string? Descricao { get; set; }

        public int Votos;

        [Required]
        [StringLength(1)]
        public string StatusSugestao { get; set; } = null!; 
        // A = Andamento, E = Espera, F = Finalizado, R = Recusado

        public DateTime DataStatus { get; set; }

        public DateTime DataSugestao { get; set; }

        public string? Imagem { get; set; }

        [Required]
        [StringLength(1)]
        public string Localizacao { get; set; } = null!;
        // T = Térreo, 1 = 1° andar, 2 = 2° andar, C = Coworking, R = Refeitório

        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; } = null!;
    }
}