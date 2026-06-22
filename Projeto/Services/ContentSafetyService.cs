using Google.GenAI;
using Google.GenAI.Types;
using Projeto.Interfaces;
using Projeto.Models;

namespace Projeto.Services;

public class ContentSafetyService : IContentSafetyService
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
Você é um moderador de conteúdo rigoroso para uma plataforma institucional de sugestões escolares/corporativas.
Analise as informações abaixo de uma sugestão sendo postada.

Nome da sugestão: {sugestao.Nome}
Descrição: {sugestao.Descricao}
{(imagem != null ? "Uma imagem também foi enviada junto. Analise-a com o mesmo critério." : "")}

REGRAS DE MODERAÇÃO — qualquer uma das situações abaixo torna o conteúdo INSEGURO:

1. LINGUAGEM OFENSIVA: palavrões, xingamentos, insultos ou qualquer palavra de baixo calão em qualquer idioma (ex: palavras como 'merda', 'porra', 'foda', 'caralho', 'buceta', 'puta', 'viado', 'idiota', 'imbecil', 'retardado', etc., mesmo que escritas com caracteres substituídos como '@', '*', '0').
2. LINGUAGEM INAPROPRIADA: termos pejorativos, discriminatórios, racistas, homofóbicos, machistas ou de ódio.
3. CONTEÚDO ILEGAL: drogas, armas, atividades ilegais.
4. CONTEÚDO SEXUAL: pornografia ou conteúdo sexualmente sugestivo.
5. EXPLORAÇÃO: exploração infantil ou violação de direitos humanos.
6. SPAM OU NONSENSE: texto sem sentido, repetição excessiva ou conteúdo claramente não relacionado a sugestões institucionais.

Responda APENAS com a palavra 'SEGURO' se todo o conteúdo for aceitável.
Se alguma regra for violada (no texto OU na imagem), responda OBRIGATORIAMENTE com o formato: 'INSEGURO: [Breve motivo em português]'.
Seja rigoroso: em caso de dúvida sobre linguagem ofensiva, classifique como INSEGURO.";

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

    public async Task<(bool IsDuplicate, string Message)> VerificarDuplicidadeAsync(Sugestao novaSugestao, IEnumerable<Sugestao> sugestoesExistentes)
    {
        if (string.IsNullOrEmpty(_apiKey))
            return (false, string.Empty);

        var lista = sugestoesExistentes.ToList();
        if (!lista.Any())
            return (false, string.Empty);

        try
        {
            var client = new Client(apiKey: _apiKey);

            // Mapa de códigos de localização para nomes legíveis
            var locaisMap = new Dictionary<string, string>
            {
                { "1", "1° Andar" }, { "2", "2° Andar" }, { "C", "Cowork" },
                { "B", "Biblioteca" }, { "R", "Refeitório" }, { "T", "Térreo" }
            };

            string LocalNome(string? cod) =>
                cod != null && locaisMap.TryGetValue(cod, out var n) ? n : (cod ?? "Não informado");

            var sugestoesTexto = string.Join("\n", lista.Select((s, i) =>
                $"[{i + 1}] Nome: \"{s.Nome}\" | Descrição: \"{s.Descricao}\" | Local: \"{LocalNome(s.Localizacao)}\""));

            var prompt = $@"
Você é um sistema de detecção de duplicidades em uma plataforma de sugestões institucionais.

Nova sugestão sendo submetida:
Nome: ""{novaSugestao.Nome}""
Descrição: ""{novaSugestao.Descricao}""
Local: ""{LocalNome(novaSugestao.Localizacao)}""

Sugestões já cadastradas no sistema:
{sugestoesTexto}

Sua tarefa: verifique se a nova sugestão trata do MESMO problema E está no MESMO local que alguma já existente.

Critérios para considerar DUPLICADA (TODOS devem ser verdadeiros):
- A sugestão existente está no MESMO local que a nova sugestão
- A solução ou melhoria pedida é equivalente ou muito similar
- Um usuário leria as duas e concluiria que tratam da mesma coisa

Critérios para considerar ÚNICA (qualquer um é suficiente):
- Os locais são DIFERENTES (ex: bebedouro do 1° Andar vs bebedouro do 2° Andar → ÚNICA)
- O tema é genuinamente diferente
- Pode ser complementar, mas não idêntico em propósito

Responda APENAS em um dos dois formatos abaixo, sem explicações adicionais:
UNICA
DUPLICADA: [número da sugestão similar entre colchetes, ex: 3] - [nome exato da sugestão similar]";

            var response = await client.Models.GenerateContentAsync(
                model: "gemini-2.5-flash-lite",
                contents: prompt
            );

            var responseText = response.Text?.Trim() ?? string.Empty;

            if (responseText.StartsWith("DUPLICADA", StringComparison.OrdinalIgnoreCase))
            {
                var detalhes = responseText.Substring("DUPLICADA".Length).Trim(':', ' ');
                return (true, $"Já existe uma sugestão semelhante cadastrada: {detalhes}. Por favor, vote na sugestão existente em vez de criar uma nova.");
            }

            return (false, string.Empty);
        }
        catch (Exception ex)
        {
            // Fail-open: se a IA falhar, permite a submissão
            return (false, $"Aviso: verificação de duplicidade indisponível ({ex.Message})");
        }
    }
}