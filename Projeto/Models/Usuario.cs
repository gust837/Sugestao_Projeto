using System.ComponentModel.DataAnnotations;

namespace Projeto.Model
{
    public class Usuario
    {
        public Usuario(string n, string c, string e, string s, bool a)
        {
            Nome = n;
            Cpf = c;
            Email = e;
            Senha = s;
            Adm = a;
        }

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

        [Required]
        [StringLength(100)]
        public string Senha { get; set; } = null!;

        public bool Adm { get; set; }
    }
}