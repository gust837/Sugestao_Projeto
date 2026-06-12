using Google.GenAI;
using Google.GenAI.Types;
using Projeto.Models;

namespace Projeto.Services;

public class ContentSafetyService
{
    private readonly string _apiKey;

    public ContentSafetyService(IConfiguration configuration)
    {
        _apiKey = configuration["Gemini:ApiKey"] ?? System.Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? string.Empty;
    }

    public async Task<(bool IsSafe, string Message)> ValidacaoSugestaoAsync(Sugestao sugestao, IFormFile? imagem = null)
    {
        if (string.IsNullOrEmpty(_apiKey))
        {
            return (true, "Aviso: API Key não configurada. Validação da IA ignorada.");
        }

        try
        {
            var client = new Client(apiKey: _apiKey);

            var prompt = $@"
Você é um moderador de conteúdo rigoroso.
Analise as informações abaixo de uma sugestão sendo postada.

Nome da sugestão: {sugestao.Nome}
Descrição: {sugestao.Descricao}
{(imagem != null ? "Uma imagem também foi enviada junto. Analise-a com o mesmo critério." : "")}

Responda APENAS com a palavra 'SEGURO' se todo o conteúdo for aceitável.
Se houver conteúdo ilegal, drogas, armas, exploração infantil, violação de direitos, ofensa ou pornografia (no texto OU na imagem), responda com o formato estrito: 'INSEGURO: [Breve motivo em português]'.";

            GenerateContentResponse response;

            if (imagem != null && imagem.Length > 0)
            {
                // Lê os bytes da imagem e converte para Base64
                using var ms = new MemoryStream();
                await imagem.CopyToAsync(ms);
                var imageBytes = ms.ToArray();

                // Monta o conteúdo multimodal (texto + imagem)
                var contents = new Content
                {
                    Parts =
                    [
                        new Part { Text = prompt },
                        new Part
                        {
                            InlineData = new Blob
                            {
                                MimeType = imagem.ContentType,
                                Data = imageBytes  // byte[] direto, sem converter para Base64
                            }
                        }
                    ]
                };

                response = await client.Models.GenerateContentAsync(
                    model: "gemini-2.5-flash-lite",
                    contents: contents
                );
            }
            else
            {
                // Sem imagem: apenas texto (comportamento original)
                response = await client.Models.GenerateContentAsync(
                    model: "gemini-2.5-flash-lite",
                    contents: prompt
                );
            }

            var responseText = response.Text?.Trim() ?? string.Empty;

            if (responseText.StartsWith("INSEGURO", StringComparison.OrdinalIgnoreCase))
            {
                var reason = responseText.Substring("INSEGURO".Length).Trim(':', ' ');
                return (false, $"Conteúdo inadequado: {reason}");
            }

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, $"Erro na validação de IA: {ex.Message}");
        }
    }
}