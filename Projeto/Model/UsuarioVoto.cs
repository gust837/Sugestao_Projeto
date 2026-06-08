using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Model
{
    public class UsuarioVoto
    {
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; } = null!;

        public int SugestaoId { get; set; }
        [ForeignKey("SugestaoId")]
        public Sugestao Sugestao { get; set; } = null!;
    }
}