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
                new("numerologia", "Numeros simbolizam ritmos, talentos latentes e formas de atravessar desafios."),
                new("arquetipos", "Arquetipos servem como espelhos poeticos da personalidade e da jornada emocional."),
                new("xamanismo", "A leitura xamanica pode falar de direcao, instinto, natureza e reconexao com a propria presenca."),
                new("seguranca", "Evite frases absolutas. Prefira linguagem interpretativa, calorosa e elegante.")
            ],
            "birthdate" =>
            [
                new("astrologia", "Signo solar representa a forma basica de irradiar identidade e vitalidade."),
                new("elementos", "Fogo inspira, terra materializa, ar conecta e agua sensibiliza."),
                new("numerologia", "O caminho de vida oferece um simbolo de aprendizados recorrentes e potenciais de alma."),
                new("seguranca", "Apresente o conteudo como autoconhecimento e entretenimento, sem certezas cientificas.")
            ],
            _ =>
            [
                new("compatibilidade", "Compatibilidade simbolica considera ressonancia, contraste fertil e aprendizado relacional."),
                new("relacoes", "Afinidade nao elimina desafios; ela mostra como duas energias podem se compor com mais consciencia."),
                new("misticismo", "Sincronicidades, arquétipos e ciclos podem enriquecer a narrativa do vinculo sem virar determinismo."),
                new("seguranca", "Nao trate a leitura como garantia de destino, permanencia ou previsao absoluta.")
            ]
        };

        return Task.FromResult(notes);
    }
}
