using System.Text;
using Oranum.Application.Models;

namespace Oranum.Application.Prompts;

public static class MysticPromptFactory
{
    public static PromptEnvelope CreateNamePrompt(NameReadingContext context)
    {
        var systemPrompt = """
Voce e o Oranum, uma inteligencia mistica, elegante e acolhedora.
Sua funcao e gerar leituras simbolicas em portugues do Brasil com tom premium, profundo e claro.
Trate tudo como experiencia interpretativa de autoconhecimento e entretenimento, sem afirmar verdade cientifica.
Nunca faca promessas medicas, diagnosticos, previsoes absolutas ou garantias.
Responda apenas com JSON valido, sem markdown, sem cercas de codigo e sem comentarios.
Use o contrato:
{
  \"nomeAnalisado\": \"string\",
  \"numeroPrincipal\": 0,
  \"tituloLeitura\": \"string\",
  \"energiaGeral\": \"string\",
  \"arquetipoPredominante\": \"string\",
  \"significadoDoNome\": \"string\",
  \"forcas\": [\"string\"],
  \"desafios\": [\"string\"],
  \"leituraXamanica\": \"string\",
  \"conselhoEspiritual\": \"string\",
  \"resumoFinal\": \"string\"
}
""";

        var userPrompt = new StringBuilder()
            .AppendLine("Analise o mapa energetico do nome abaixo.")
            .AppendLine($"Nome informado: {context.FullName}")
            .AppendLine($"Numero principal calculado internamente: {context.Numerology.PrincipalNumber}")
            .AppendLine($"Soma numerologica: {context.Numerology.RawSum}")
            .AppendLine($"Significado simbolico-base: {context.Numerology.SymbolicMeaning}")
            .AppendLine($"Arquetipo-base: {context.Numerology.PredominantArchetype}")
            .AppendLine($"Energia-base: {context.Numerology.EnergySignature}")
            .AppendLine($"Pistas de forca: {string.Join(", ", context.Numerology.StrengthHints)}")
            .AppendLine($"Pistas de desafio: {string.Join(", ", context.Numerology.ChallengeHints)}")
            .AppendLine("Use os conhecimentos internos abaixo como repertorio complementar:")
            .Append(RenderKnowledge(context.KnowledgeNotes))
            .AppendLine("Personalize o texto para soar unico e compartilhavel.")
            .ToString();

        return new PromptEnvelope(systemPrompt, userPrompt);
    }

    public static PromptEnvelope CreateBirthPrompt(BirthDateReadingContext context)
    {
        var systemPrompt = """
Voce e o Oranum, uma inteligencia mistica, elegante e acolhedora.
Gere uma leitura simbolica em portugues do Brasil baseada em astrologia basica e numerologia.
O texto deve ser interpretativo, sofisticado e acessivel, sem alegar verdade cientifica.
Nao use markdown. Responda apenas com JSON valido, seguindo exatamente este contrato:
{
  \"dataNascimento\": \"string\",
  \"signoSolar\": \"string\",
  \"elemento\": \"string\",
  \"caminhoDeVida\": 0,
  \"energiaCentral\": \"string\",
  \"tendenciasEmocionais\": \"string\",
  \"missaoDeVida\": \"string\",
  \"desafios\": [\"string\"],
  \"potenciais\": [\"string\"],
  \"conselhoFinal\": \"string\"
}
""";

        var birth = context.BirthProfile;
        var userPrompt = new StringBuilder()
            .AppendLine("Crie uma leitura simbolica da data de nascimento.")
            .AppendLine($"Nome da pessoa: {context.FullName}")
            .AppendLine($"Data de nascimento: {birth.BirthDate:yyyy-MM-dd}")
            .AppendLine($"Signo calculado internamente: {birth.ZodiacSign}")
            .AppendLine($"Elemento calculado internamente: {birth.Element}")
            .AppendLine($"Caminho de vida calculado internamente: {birth.LifePathNumber}")
            .AppendLine($"Energia-base: {birth.CentralEnergy}")
            .AppendLine($"Sintese simbolica: {birth.SymbolicProfile}")
            .AppendLine($"Missao-base: {birth.Mission}")
            .AppendLine($"Desafios-base: {string.Join(", ", birth.ChallengeHints)}")
            .AppendLine($"Potenciais-base: {string.Join(", ", birth.PotentialHints)}")
            .AppendLine("Conhecimento interno adicional:")
            .Append(RenderKnowledge(context.KnowledgeNotes))
            .ToString();

        return new PromptEnvelope(systemPrompt, userPrompt);
    }

    public static PromptEnvelope CreateCompatibilityPrompt(CompatibilityReadingContext context)
    {
        var systemPrompt = """
Voce e o Oranum, uma inteligencia mistica, elegante e acolhedora.
Sua tarefa e interpretar compatibilidade entre duas pessoas com linguagem clara, premium e simbolica.
O conteudo e de autoconhecimento e entretenimento, sem promessas absolutas.
Responda somente com JSON valido e sem markdown, obedecendo exatamente o contrato:
{
  \"pessoa1\": \"string\",
  \"pessoa2\": \"string\",
  \"compatibilidadePercentual\": 0,
  \"nivelCompatibilidade\": \"string\",
  \"afinidadeEnergetica\": \"string\",
  \"afinidadeEmocional\": \"string\",
  \"afinidadeEspiritual\": \"string\",
  \"pontosFortes\": [\"string\"],
  \"pontosDeAtencao\": [\"string\"],
  \"conselhoRelacional\": \"string\",
  \"resumoVinculo\": \"string\"
}
""";

        var userPrompt = new StringBuilder()
            .AppendLine("Interprete a compatibilidade entre as duas pessoas abaixo.")
            .AppendLine($"Pessoa 1: {context.Person1Name}")
            .AppendLine($"Numero principal da pessoa 1: {context.Profile1.PrincipalNumber}")
            .AppendLine($"Arquetipo da pessoa 1: {context.Profile1.PredominantArchetype}")
            .AppendLine($"Pessoa 2: {context.Person2Name}")
            .AppendLine($"Numero principal da pessoa 2: {context.Profile2.PrincipalNumber}")
            .AppendLine($"Arquetipo da pessoa 2: {context.Profile2.PredominantArchetype}")
            .AppendLine($"Score base deterministico: {context.CompatibilityProfile.CompatibilityScore}")
            .AppendLine($"Nivel base: {context.CompatibilityProfile.CompatibilityLevel}")
            .AppendLine($"Afinidade energetica-base: {context.CompatibilityProfile.EnergeticAffinity}")
            .AppendLine($"Afinidade emocional-base: {context.CompatibilityProfile.EmotionalAffinity}")
            .AppendLine($"Afinidade espiritual-base: {context.CompatibilityProfile.SpiritualAffinity}")
            .AppendLine($"Pontos fortes-base: {string.Join(", ", context.CompatibilityProfile.StrengthHints)}")
            .AppendLine($"Pontos de atencao-base: {string.Join(", ", context.CompatibilityProfile.AttentionHints)}")
            .AppendLine($"Ponto de equilibrio-base: {context.CompatibilityProfile.BalanceGuidance}")
            .AppendLine($"Dados adicionais da pessoa 1: {RenderBirth(context.BirthProfile1)}")
            .AppendLine($"Dados adicionais da pessoa 2: {RenderBirth(context.BirthProfile2)}")
            .AppendLine("Conhecimento interno adicional:")
            .Append(RenderKnowledge(context.KnowledgeNotes))
            .ToString();

        return new PromptEnvelope(systemPrompt, userPrompt);
    }

    private static string RenderKnowledge(IEnumerable<MysticKnowledgeNote> notes) =>
        string.Join(Environment.NewLine, notes.Select(note => $"- {note.Source}: {note.Content}"));

    private static string RenderBirth(Oranum.Domain.ValueObjects.BirthProfile? birthProfile) =>
        birthProfile is null
            ? "data nao informada"
            : $"signo {birthProfile.ZodiacSign}, elemento {birthProfile.Element}, caminho {birthProfile.LifePathNumber}";
}
