using Google.GenAI;
using Projeto.Models;

namespace Projeto.Services;

public class ContentSafetyService
{
    private readonly string _apiKey;

    public ContentSafetyService(IConfiguration configuration)
    {
        _apiKey = configuration["Gemini:ApiKey"] ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? string.Empty;
    }

    public async Task<(bool IsSafe, string Message)> ValidateProductAsync(Sugestao sugestao)
    {
        if (string.IsNullOrEmpty(_apiKey))
        {
            return (true, "Aviso: API Key não configurada. Validação da IA ignorada.");
        }

        try
        {
            // Inicializando o client do Google.GenAI
            var client = new Client(apiKey: _apiKey);

            var prompt = $@"
Você é um moderador de conteúdo rigoroso.
Analise as informações abaixo de uma sugestão sendo postada.

Nome da sugestão: {sugestao.Nome}
Descrição: {sugestao.Descricao}

Responda APENAS com a palavra 'SEGURO' se a sugestão for aceitável.
Se houver conteúdo ilegal, drogas, armas, exploração infantil, violação de direitos, ofensa ou pornografia, responda com o formato estrito: 'INSEGURO: [Breve motivo em português]'.";

            var response = await client.Models.GenerateContentAsync(
                model: "gemini-2.5-flash-lite",
                contents: prompt
            );

            // A resposta é acessada nas properties Text primárias
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
