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

        public SugestaoService(ISugestaoRepository repository)
        {
            _repository = repository;
        }

        public async Task CriarSugestao(Sugestao s, string? categorias, IFormFile arquivoImagem)
        {
            if (arquivoImagem != null && arquivoImagem.Length > 0)
            {
                s.Imagem = await UploadImagemAsync(arquivoImagem);
            }
            else
            {
                s.Imagem = "";
            }

            await _repository.CriarSugestao(s);

            if (!string.IsNullOrEmpty(categorias))
            {
                var categoriasIds = categorias.Split(",").Select(id => int.TryParse(id, out var convertido) ? convertido : 0).Where(id => id > 0).ToList();

                foreach (var catId in categoriasIds)
                {
                    Sugestao_Categoria sc = new Sugestao_Categoria
                    {
                        SugestaoId = s.Id,
                        CategoriaId = catId
                    };
                    await _repository.CriarSugCat(sc);
                }
            }
        }

        public async Task EditarStatusSugestao(int sugId, string status)
        {
            var sug = await _repository.ProcurarSugestao(sugId);

            sug.StatusSugestao = status;

            await _repository.EditarSugestao(sug);
        }

        public async Task ExcluirSugestao(int sugId)
        {
            var sug = await _repository.ProcurarSugestao(sugId);
            if (sug != null)
            {
                await _repository.ExcluirSugCat(sugId);
                await _repository.ExcluirSugestao(sug);
            }
        }

        public async Task<IEnumerable<Sugestao>> ListarSugestaoPorCategoria(int categoriaId)
        {
            return await _repository.ListarSugestaoPorCategoria(categoriaId);
        }

        public async Task<IEnumerable<Sugestao>> ListarSugestaoPorStatus(string status)
        {
            return await _repository.ListarSugestaoPorStatus(status);
        }

        public async Task<IEnumerable<Sugestao>> ListarSugestoes()
        {
            return await _repository.ListarSugestoes();
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
                await _repository.RemoverVoto(sug, new Usuario_Voto(usuarioId, postId));
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