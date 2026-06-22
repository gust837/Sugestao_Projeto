using System.ComponentModel.DataAnnotations;

namespace Projeto.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = null!;

        public ICollection<Sugestao_Categoria> Sugestao_Categorias { get; set; }= new List<Sugestao_Categoria>();
         
    }
}