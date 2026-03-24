import { ChevronDown } from "lucide-react";
import { SectionHeading } from "@/components/SectionHeading";

const items = [
  {
    question: "Como a leitura e criada?",
    answer: "O Oranum combina calculos deterministicos de numerologia, leitura simbolica da data e um enriquecimento narrativo com IA para transformar os sinais em uma experiencia elegante.",
  },
  {
    question: "Preciso informar a data de nascimento logo no inicio?",
    answer: "Nao. Voce pode revelar primeiro o mapa energetico do nome e depois desbloquear a camada complementar com signo solar, caminho de vida e tendencias simbolicas.",
  },
  {
    question: "A compatibilidade depende de duas datas?",
    answer: "Ela funciona apenas com os nomes, mas fica mais rica quando voce adiciona as datas de nascimento para ampliar a leitura de afinidade emocional e espiritual.",
  },
  {
    question: "Isso e uma verdade absoluta?",
    answer: "Nao. O conteudo e interpretativo e voltado para autoconhecimento e entretenimento, com linguagem mistica e narrativa personalizada.",
  },
];

export function FAQSection() {
  return (
    <section id="faq" className="section-space pt-8">
      <div className="container-shell space-y-10">
        <SectionHeading
          eyebrow="FAQ"
          title="Perguntas frequentes sobre a experiencia Oranum"
          description="Tudo foi desenhado para ser simples na entrada, encantador no percurso e sofisticado no resultado final."
          align="center"
        />
        <div className="mx-auto max-w-4xl space-y-4">
          {items.map((item) => (
            <details key={item.question} className="glass-panel group p-6 open:border-gold/20">
              <summary className="flex cursor-pointer list-none items-center justify-between gap-4 text-left text-lg font-semibold text-white">
                {item.question}
                <ChevronDown className="size-5 text-gold transition group-open:rotate-180" />
              </summary>
              <p className="mt-4 text-sm leading-7 text-mist/85 sm:text-base">{item.answer}</p>
            </details>
          ))}
        </div>
      </div>
    </section>
  );
}
