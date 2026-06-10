using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class SugestaoCategoria
    {
        public int SugestaoId { get; set; }
        [ForeignKey("SugestaoId")]
        public Sugestao Sugestao { get; set; } = null!;

        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; } = null!;
    }
}