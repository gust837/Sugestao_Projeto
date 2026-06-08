using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Model
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