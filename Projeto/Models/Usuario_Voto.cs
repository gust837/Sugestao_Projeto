using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class Usuario_Voto
    {
        protected Usuario_Voto(){}
        public Usuario_Voto(int usuarioId, int postId)
        {
            UsuarioId = usuarioId;
            SugestaoId = postId;
        }
        
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; } = null!;

        public int SugestaoId { get; set; }
        [ForeignKey("SugestaoId")]
        public Sugestao Sugestao { get; set; } = null!;
    }
}