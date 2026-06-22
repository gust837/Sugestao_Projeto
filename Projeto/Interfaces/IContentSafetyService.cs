using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projeto.Models;

namespace Projeto.Interfaces
{
    public interface IContentSafetyService
    {
        Task<(bool IsSafe, string Message)> ValidacaoSugestaoAsync(Sugestao sugestao, IFormFile? imagem = null);
        Task<(bool IsDuplicate, string Message)> VerificarDuplicidadeAsync(Sugestao novaSugestao, IEnumerable<Sugestao> sugestoesExistentes);
    }
}