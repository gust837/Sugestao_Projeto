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
         
    }
}