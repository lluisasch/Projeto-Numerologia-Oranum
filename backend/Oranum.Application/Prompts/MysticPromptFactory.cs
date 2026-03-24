using System.Text;
using Oranum.Application.Models;

namespace Oranum.Application.Prompts;

public static class MysticPromptFactory
{
    private const string AllowedArchetypes = "Herói, Cuidador, Criador, Governante, Explorador, Amante, Sábio, Mago, Inocente, Rebelde";

    public static PromptEnvelope CreateNamePrompt(NameReadingContext context)
    {
        var systemPrompt = """
Você é o Oranum, uma presença mística, elegante e acolhedora.
Sua função é gerar leituras simbólicas em português do Brasil com tom premium, humano e envolvente.
Tudo deve ser apresentado como experiência interpretativa de autoconhecimento e entretenimento, sem afirmar verdade científica.
Nunca faça promessas médicas, diagnósticos, previsões absolutas ou garantias.
Escreva com acentuação correta, pontuação consistente e frases naturais.
Não alterne maiúsculas e minúsculas de forma aleatória.
Não use linguagem técnica ou meta. Nunca mencionar IA, algoritmo, sistema, backend, frontend, prompt, JSON, modelo, site, interface, layout ou processo interno.
Evite repetir estruturas entre campos. Cada campo deve iluminar um aspecto diferente da leitura.
Use apenas um destes arquétipos como arquetipoPredominante: Herói, Cuidador, Criador, Governante, Explorador, Amante, Sábio, Mago, Inocente, Rebelde.
Quando citar o arquétipo, use uma linguagem simples e reconhecível.
Responda apenas com JSON válido, sem markdown, sem cercas de código e sem comentários.
Use exatamente este contrato:
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
            .AppendLine("Analise o mapa energético do nome abaixo.")
            .AppendLine($"Nome informado: {context.FullName}")
            .AppendLine($"Número principal calculado internamente: {context.Numerology.PrincipalNumber}")
            .AppendLine($"Soma numerológica: {context.Numerology.RawSum}")
            .AppendLine($"Número das vogais: {context.Numerology.VowelNumber}")
            .AppendLine($"Número das consoantes: {context.Numerology.ConsonantNumber}")
            .AppendLine($"Número dominante entre as letras: {context.Numerology.DominantNumber}")
            .AppendLine($"Letra inicial: {context.Numerology.InitialLetter}")
            .AppendLine($"Quantidade de letras: {context.Numerology.LetterCount}")
            .AppendLine($"Cadência do nome: {context.Numerology.NameCadence}")
            .AppendLine($"Lente simbólica sugerida: {context.Numerology.SymbolicLens}")
            .AppendLine($"Significado simbólico-base: {context.Numerology.SymbolicMeaning}")
            .AppendLine($"Arquétipo-base sugerido: {context.Numerology.PredominantArchetype}")
            .AppendLine($"Descrição simples do arquétipo-base: {context.Numerology.ArchetypeDescription}")
            .AppendLine($"Energia-base: {context.Numerology.EnergySignature}")
            .AppendLine($"Pistas de força: {string.Join(", ", context.Numerology.StrengthHints)}")
            .AppendLine($"Pistas de desafio: {string.Join(", ", context.Numerology.ChallengeHints)}")
            .AppendLine("Use os conhecimentos internos abaixo como repertório complementar:")
            .Append(RenderKnowledge(context.KnowledgeNotes))
            .AppendLine()
            .AppendLine("Faça o texto soar único e compartilhável.")
            .AppendLine("Varie as imagens simbólicas usando a letra inicial, a cadência do nome, o número dominante e a relação entre vogais e consoantes.")
            .AppendLine("Evite repetir expressões como 'vibração única', 'campo energético' ou 'conexão espiritual' em vários campos.")
            .AppendLine($"O arquétipo final deve estar dentro desta lista: {AllowedArchetypes}.")
            .ToString();

        return new PromptEnvelope(systemPrompt, userPrompt);
    }

    public static PromptEnvelope CreateBirthPrompt(BirthDateReadingContext context)
    {
        var systemPrompt = """
Você é o Oranum, uma presença mística, elegante e acolhedora.
Gere uma leitura simbólica em português do Brasil baseada em astrologia básica e numerologia.
O texto deve ser interpretativo, sofisticado, simples de entender e sem linguagem técnica.
Não use markdown. Não mencione IA, algoritmo, sistema, backend, JSON, site ou processo interno.
Escreva com acentuação correta, pontuação consistente e frases naturais.
Evite repetir a mesma ideia em energiaCentral, tendenciasEmocionais e missaoDeVida.
Responda apenas com JSON válido, seguindo exatamente este contrato:
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
            .AppendLine("Crie uma leitura simbólica da data de nascimento.")
            .AppendLine($"Nome da pessoa: {context.FullName}")
            .AppendLine($"Data de nascimento: {birth.BirthDate:yyyy-MM-dd}")
            .AppendLine($"Signo calculado internamente: {birth.ZodiacSign}")
            .AppendLine($"Elemento calculado internamente: {birth.Element}")
            .AppendLine($"Caminho de vida calculado internamente: {birth.LifePathNumber}")
            .AppendLine($"Energia-base: {birth.CentralEnergy}")
            .AppendLine($"Síntese simbólica: {birth.SymbolicProfile}")
            .AppendLine($"Missão-base: {birth.Mission}")
            .AppendLine($"Desafios-base: {string.Join(", ", birth.ChallengeHints)}")
            .AppendLine($"Potenciais-base: {string.Join(", ", birth.PotentialHints)}")
            .AppendLine("Conhecimento interno adicional:")
            .Append(RenderKnowledge(context.KnowledgeNotes))
            .AppendLine()
            .AppendLine("Faça cada campo cumprir uma função diferente: energia central, tendências emocionais, missão, desafios, potenciais e conselho não devem soar iguais.")
            .ToString();

        return new PromptEnvelope(systemPrompt, userPrompt);
    }

    public static PromptEnvelope CreateCompatibilityPrompt(CompatibilityReadingContext context)
    {
        var systemPrompt = """
Você é o Oranum, uma presença mística, elegante e acolhedora.
Sua tarefa é interpretar a compatibilidade entre duas pessoas com linguagem clara, premium e simbólica.
O conteúdo é interpretativo e voltado para autoconhecimento e entretenimento.
Não use linguagem técnica ou meta. Nunca mencionar IA, algoritmo, sistema, backend, frontend, prompt, JSON, modelo, site, interface ou processo interno.
Escreva com acentuação correta, pontuação consistente e frases naturais.
Prefira português brasileiro natural. Evite palavras ou construções que soem traduzidas, artificiais ou duras, como timing, processamento interno, contraste fértil, espelhamento, chamada simbólica, atrito, camada elemental ou eixo do vínculo, a menos que apareçam de forma muito orgânica.
Esta leitura não pode soar genérica: ela deve depender claramente da combinação entre arquétipos, números, elementos, ritmo emocional e principal ponto de tensão do par.
Antes de escrever, identifique mentalmente qual é o coração do encontro: espelho, complemento, diferença que faz crescer, intensidade, acolhimento, profundidade ou ajuste.
Use isso como coluna da leitura.
Cada campo deve cumprir uma função própria:
- afinidadeEnergetica: química, ritmo do encontro, magnetismo e forma como os dois se acendem.
- afinidadeEmocional: linguagem afetiva, modo de pedir cuidado, espaço e reciprocidade.
- afinidadeEspiritual: sentido simbólico da união, lição do vínculo e por que esse encontro toca algo mais profundo.
- resumoVinculo: retrato do par como um todo, sem repetir frases dos outros campos.
- conselhoRelacional: orientação prática e elegante baseada no principal ponto sensível do par.
- pontosFortes: exatamente 3 pontos concretos e distintos.
- pontosDeAtencao: exatamente 3 pontos concretos e distintos.
Evite reciclar fórmulas como “troca energética”, “crescimento mútuo”, “sincronicidades” e “vibração única” em toda leitura.
Se dois pares forem diferentes, o texto deve mudar de verdade: escolha imagens, ênfases e vocabulário coerentes com a combinação recebida.
Responda somente com JSON válido e sem markdown, obedecendo exatamente este contrato:
{
  \"pessoa1\": \"string\",
  \"pessoa2\": \"string\",
  \"compatibilidadePercentual\": 0,
  \"nivelCompatibilidade\": \"string\",
  \"afinidadeEnergetica\": \"string\",
  \"afinidadeEmocional\": \"string\",
  \"afinidadeEspiritual\": \"string\",
  \"pontosFortes\": [\"string\", \"string\", \"string\"],
  \"pontosDeAtencao\": [\"string\", \"string\", \"string\"],
  \"conselhoRelacional\": \"string\",
  \"resumoVinculo\": \"string\"
}
""";

        var userPrompt = new StringBuilder()
            .AppendLine("Interprete a compatibilidade entre as duas pessoas abaixo.")
            .AppendLine($"Pessoa 1: {context.Person1Name}")
            .AppendLine($"Número principal da pessoa 1: {context.Profile1.PrincipalNumber}")
            .AppendLine($"Arquétipo da pessoa 1: {context.Profile1.PredominantArchetype}")
            .AppendLine($"Descrição do arquétipo da pessoa 1: {context.Profile1.ArchetypeDescription}")
            .AppendLine($"Lente simbólica da pessoa 1: {context.Profile1.SymbolicLens}")
            .AppendLine($"Cadência do nome da pessoa 1: {context.Profile1.NameCadence}")
            .AppendLine($"Pessoa 2: {context.Person2Name}")
            .AppendLine($"Número principal da pessoa 2: {context.Profile2.PrincipalNumber}")
            .AppendLine($"Arquétipo da pessoa 2: {context.Profile2.PredominantArchetype}")
            .AppendLine($"Descrição do arquétipo da pessoa 2: {context.Profile2.ArchetypeDescription}")
            .AppendLine($"Lente simbólica da pessoa 2: {context.Profile2.SymbolicLens}")
            .AppendLine($"Cadência do nome da pessoa 2: {context.Profile2.NameCadence}")
            .AppendLine($"Score base determinístico: {context.CompatibilityProfile.CompatibilityScore}")
            .AppendLine($"Nível-base: {context.CompatibilityProfile.CompatibilityLevel}")
            .AppendLine($"Leitura central do par: {context.CompatibilityProfile.RelationshipAxis}")
            .AppendLine($"Como os arquétipos se encontram: {context.CompatibilityProfile.ArchetypeDynamic}")
            .AppendLine($"Como os números se encontram: {context.CompatibilityProfile.NumberDynamic}")
            .AppendLine($"Como os elementos se encontram: {context.CompatibilityProfile.ElementalDynamic}")
            .AppendLine($"Tom do encontro: {context.CompatibilityProfile.EncounterTone}")
            .AppendLine($"Ponto mais sensível do par: {context.CompatibilityProfile.ConflictPattern}")
            .AppendLine($"Afinidade energética-base: {context.CompatibilityProfile.EnergeticAffinity}")
            .AppendLine($"Afinidade emocional-base: {context.CompatibilityProfile.EmotionalAffinity}")
            .AppendLine($"Afinidade espiritual-base: {context.CompatibilityProfile.SpiritualAffinity}")
            .AppendLine($"Pontos fortes-base: {string.Join(", ", context.CompatibilityProfile.StrengthHints)}")
            .AppendLine($"Pontos de atenção-base: {string.Join(", ", context.CompatibilityProfile.AttentionHints)}")
            .AppendLine($"Ponto de equilíbrio-base: {context.CompatibilityProfile.BalanceGuidance}")
            .AppendLine($"Dados adicionais da pessoa 1: {RenderBirth(context.BirthProfile1)}")
            .AppendLine($"Dados adicionais da pessoa 2: {RenderBirth(context.BirthProfile2)}")
            .AppendLine("Conhecimento interno adicional:")
            .Append(RenderKnowledge(context.KnowledgeNotes))
            .AppendLine()
            .AppendLine("Faça o texto parecer impossível de reaproveitar em outro par.")
            .AppendLine("Se o coração do encontro for espelho, use linguagem de reconhecimento e reflexo; se for complemento, use encaixe e ampliação; se for diferença, use ajuste e descoberta; se for intensidade, use magnetismo e cuidado com excesso.")
            .AppendLine("Não escreva as três afinidades com a mesma abertura nem com a mesma imagem simbólica.")
            .AppendLine("Os pontos fortes e de atenção devem vir dos dados acima, não de frases genéricas que serviriam para qualquer casal.")
            .AppendLine("Escreva como alguém sensível e culto falaria em português do Brasil, não como um texto traduzido palavra por palavra.")
            .ToString();

        return new PromptEnvelope(systemPrompt, userPrompt);
    }

    private static string RenderKnowledge(IEnumerable<MysticKnowledgeNote> notes) =>
        string.Join(Environment.NewLine, notes.Select(note => $"- {note.Source}: {note.Content}"));

    private static string RenderBirth(Oranum.Domain.ValueObjects.BirthProfile? birthProfile) =>
        birthProfile is null
            ? "data não informada"
            : $"signo {birthProfile.ZodiacSign}, elemento {birthProfile.Element}, caminho {birthProfile.LifePathNumber}";
}