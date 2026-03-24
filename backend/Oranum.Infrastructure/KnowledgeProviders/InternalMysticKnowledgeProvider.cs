using Oranum.Application.Abstractions;
using Oranum.Application.Models;

namespace Oranum.Infrastructure.KnowledgeProviders;

public sealed class InternalMysticKnowledgeProvider : IKnowledgeProvider
{
    public Task<IReadOnlyList<MysticKnowledgeNote>> GetNotesAsync(string topic, CancellationToken cancellationToken)
    {
        IReadOnlyList<MysticKnowledgeNote> notes = topic switch
        {
            "name" =>
            [
                new("numerologia", "Números simbolizam ritmos, talentos latentes e formas de atravessar desafios."),
                new("arquétipos", "Arquétipos funcionam como espelhos poéticos da personalidade e da jornada emocional."),
                new("xamanismo", "A leitura xamânica pode falar de direção, instinto, natureza e reconexão com a própria presença."),
                new("segurança", "Evite frases absolutas. Prefira linguagem interpretativa, calorosa e elegante.")
            ],
            "birthdate" =>
            [
                new("astrologia", "O signo solar representa a forma básica de irradiar identidade e vitalidade."),
                new("elementos", "Fogo inspira, terra materializa, ar conecta e água sensibiliza."),
                new("numerologia", "O caminho de vida oferece um símbolo de aprendizados recorrentes e potenciais de alma."),
                new("segurança", "Apresente o conteúdo como autoconhecimento e entretenimento, sem certezas científicas.")
            ],
            _ =>
            [
                new("compatibilidade", "Compatibilidade simbólica considera ressonância, contraste fértil, espelhamento e aprendizado relacional."),
                new("relações", "Afinidade não elimina desafios; ela mostra como duas energias podem se compor, se provocar ou se amadurecer com mais consciência."),
                new("dinâmica", "Alguns pares pedem linguagem de abrigo, outros de magnetismo, outros de ajuste fino. A narrativa deve mudar conforme o tipo de encontro."),
                new("segurança", "Não trate a leitura como garantia de destino, permanência ou previsão absoluta.")
            ]
        };

        return Task.FromResult(notes);
    }
}