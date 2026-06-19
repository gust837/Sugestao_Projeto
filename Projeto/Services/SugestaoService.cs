using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projeto.Interfaces;
using Projeto.Models;

namespace Projeto.Services
{
    public class SugestaoService : ISugestaoService
    {
        private readonly ISugestaoRepository _repository;
        private readonly IContentSafetyService _contentSafetyService;

        public SugestaoService(ISugestaoRepository repository, IContentSafetyService contentSafetyService)
        {
            _repository = repository;
            _contentSafetyService = contentSafetyService;
        }

        public async Task<(bool Ok, string Mensagem)> CriarSugestao(Sugestao s, string? categorias, IFormFile? arquivoImagem)
        {
            var safetyCheck = await _contentSafetyService.ValidacaoSugestaoAsync(s, arquivoImagem);
            if (!safetyCheck.IsSafe)
                return (false, safetyCheck.Message);

            var sugestoesExistentes = await _repository.ListarSugestoes();
            var duplicidadeCheck = await _contentSafetyService.VerificarDuplicidadeAsync(s, sugestoesExistentes);
            if (duplicidadeCheck.IsDuplicate)
                return (false, duplicidadeCheck.Message);

            if (arquivoImagem != null && arquivoImagem.Length > 0)
                s.Imagem = await UploadImagemAsync(arquivoImagem);
            else
                s.Imagem = string.Empty;

            await _repository.CriarSugestao(s);

            if (!string.IsNullOrEmpty(categorias))
            {
                var categoriasIds = categorias
                    .Split(",")
                    .Select(id => int.TryParse(id, out var v) ? v : 0)
                    .Where(id => id > 0)
                    .ToList();

                foreach (var catId in categoriasIds)
                {
                    await _repository.CriarSugCat(new Sugestao_Categoria
                    {
                        SugestaoId = s.Id,
                        CategoriaId = catId
                    });
                }
            }

            return (true, "Sugestão cadastrada com sucesso!");
        }

        public async Task EditarStatusSugestao(int sugId, string status)
        {
            Sugestao? sug = await _repository.ProcurarSugestao(sugId);
            if (sug == null) return;

            sug.StatusSugestao = status;

            await _repository.EditarSugestao(sug);
        }

        public async Task<bool> ExcluirSugestao(int sugId)
        {
            Sugestao? sug = await _repository.ProcurarSugestao(sugId);
            if (sug == null) return false;

            await _repository.ExcluirSugCat(sugId);
            await _repository.ExcluirVotos(sugId);
            await _repository.ExcluirComentarios(sugId);
            await _repository.ExcluirSugestao(sug);

            return true;
        }

        public async Task<IEnumerable<Sugestao>> ListarSugestaoPorStatus(string status)
        {
            return await _repository.ListarSugestaoPorStatus(status);
        }

        public async Task<IEnumerable<Sugestao>> ListarSugestoes()
        {
            return await _repository.ListarSugestoes();
        }

        public async Task<IEnumerable<Categoria>> ListarCategorias()
        {
            return await _repository.ListarCategorias();
        }

        public async Task Votar(int usuarioId, int postId)
        {
            var sug = await _repository.ProcurarSugestao(postId);

            if (!await _repository.VerificarUsuarioVoto(usuarioId, postId))
            {
                sug.Votos++;
                await _repository.Votar(sug, new Usuario_Voto(usuarioId, postId));
            }
            else
            {
                sug.Votos--;
                await _repository.RemoverVoto(sug, usuarioId, postId);
            }
        }

        private async Task<string> UploadImagemAsync(IFormFile arquivoImagem)
        {
            string caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "imgSugestoes");

            if (!Directory.Exists(caminhoPasta)) Directory.CreateDirectory(caminhoPasta);

            var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(arquivoImagem.FileName);

            var caminhoArquivo = Path.Combine(caminhoPasta, nomeArquivo);

            using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
            {
                await arquivoImagem.CopyToAsync(stream);
            }

            return $"img/imgSugestoes/{nomeArquivo}";
        }
    }
}