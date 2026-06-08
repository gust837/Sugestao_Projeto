using System.ComponentModel.DataAnnotations;

namespace Projeto.Model
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Cpf { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        public bool Adm { get; set; }
    }
}