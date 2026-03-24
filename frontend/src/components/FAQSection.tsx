import { ChevronDown } from "lucide-react";
import { SectionHeading } from "@/components/SectionHeading";

const items = [
  {
    question: "Como a leitura é criada?",
    answer: "O Oranum combina símbolos do nome, referências clássicas de arquétipos, numerologia e, quando você quiser, a sua data de nascimento para compor uma leitura mais rica e pessoal.",
  },
  {
    question: "Preciso informar a data de nascimento logo no início?",
    answer: "Não. Você pode revelar primeiro o mapa energético do nome e depois desbloquear a camada complementar com signo solar, caminho de vida e tendências simbólicas.",
  },
  {
    question: "A compatibilidade depende de duas datas?",
    answer: "Ela funciona apenas com os nomes, mas fica mais rica quando você adiciona as datas de nascimento para ampliar a leitura de afinidade emocional e espiritual.",
  },
  {
    question: "Isso é uma verdade absoluta?",
    answer: "Não. O conteúdo é interpretativo e voltado para autoconhecimento e entretenimento, com linguagem mística e narrativa personalizada.",
  },
];

export function FAQSection() {
  return (
    <section id="faq" className="section-space pt-8">
      <div className="container-shell space-y-10">
        <SectionHeading
          eyebrow="FAQ"
          title="Perguntas frequentes sobre a experiência Oranum"
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