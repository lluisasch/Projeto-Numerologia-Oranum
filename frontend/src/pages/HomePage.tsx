import { motion } from "framer-motion";
import { Compass, Gem, Orbit, Sparkles, WandSparkles } from "lucide-react";
import { FAQSection } from "@/components/FAQSection";
import { HeroSection } from "@/components/HeroSection";
import { MysticalCard } from "@/components/MysticalCard";
import { SectionHeading } from "@/components/SectionHeading";
import { TestimonialSection } from "@/components/TestimonialSection";
import { usePageMeta } from "@/hooks/usePageMeta";

const features = [
  {
    icon: Sparkles,
    title: "Energia do nome",
    description: "Um retrato simbólico da vibração que seu nome projeta, com numerologia, forças e nuances de presença.",
  },
  {
    icon: Compass,
    title: "Missão pela data",
    description: "Signo solar, elemento, caminho de vida e uma síntese elegante entre astrologia básica e leitura interpretativa.",
  },
  {
    icon: Gem,
    title: "Compatibilidade refinada",
    description: "Afinidade energética, emocional e espiritual em uma leitura bonita, clara e altamente compartilhável.",
  },
];

const steps = [
  {
    title: "Digite o nome",
    description: "O ritual começa pelo nome que você deseja revelar, sem fricção e em poucos segundos.",
  },
  {
    title: "Seu mapa ganha profundidade",
    description: "Símbolos, ritmos e referências conhecidas se unem para criar uma leitura mais pessoal, elegante e envolvente.",
  },
  {
    title: "Receba sua leitura",
    description: "O resultado chega em um visual premium, pronto para virar conversa, insight e compartilhamento.",
  },
];

export function HomePage() {
  usePageMeta(
    "Oranum | Seu nome revela a sua vibração",
    "Descubra o mapa energético do seu nome com uma experiência mística premium, numerologia simbólica e leituras elegantes.",
  );

  return (
    <>
      <HeroSection />

      <section className="section-space pt-8">
        <div className="container-shell space-y-10">
          <SectionHeading
            eyebrow="O que você recebe"
            title="Uma jornada simbólica desenhada para causar encantamento e clareza"
            description="Oranum traduz sinais em linguagem sofisticada: profunda o bastante para envolver, simples o bastante para ser lida de imediato."
          />
          <div className="grid gap-6 lg:grid-cols-3">
            {features.map((feature, index) => {
              const Icon = feature.icon;
              return (
                <motion.div key={feature.title} initial={{ opacity: 0, y: 18 }} whileInView={{ opacity: 1, y: 0 }} viewport={{ once: true, amount: 0.25 }} transition={{ duration: 0.55, delay: index * 0.08 }}>
                  <MysticalCard className="h-full">
                    <div className="inline-flex rounded-2xl border border-gold/20 bg-gold/10 p-3 text-gold">
                      <Icon className="size-5" />
                    </div>
                    <h3 className="mt-5 font-display text-3xl text-white">{feature.title}</h3>
                    <p className="mt-4 text-sm leading-7 text-mist/85 sm:text-base">{feature.description}</p>
                  </MysticalCard>
                </motion.div>
              );
            })}
          </div>
        </div>
      </section>

      <section id="como-funciona" className="section-space">
        <div className="container-shell grid gap-10 lg:grid-cols-[0.95fr_1.05fr] lg:items-center">
          <SectionHeading
            eyebrow="Como funciona"
            title="Uma leitura que combina profundidade simbólica com clareza imediata"
            description="A experiência foi pensada para soar pessoal, sofisticada e envolvente do primeiro toque ao resultado final."
          />
          <div className="space-y-5">
            {steps.map((step, index) => (
              <MysticalCard key={step.title} className="flex gap-5">
                <div className="flex size-12 shrink-0 items-center justify-center rounded-2xl border border-gold/20 bg-gold/10 font-display text-2xl text-white">{index + 1}</div>
                <div>
                  <h3 className="text-xl font-semibold text-white">{step.title}</h3>
                  <p className="mt-2 text-sm leading-7 text-mist/85 sm:text-base">{step.description}</p>
                </div>
              </MysticalCard>
            ))}
          </div>
        </div>
      </section>

      <section className="section-space pt-2">
        <div className="container-shell">
          <MysticalCard className="relative overflow-hidden border-gold/10 bg-gradient-to-br from-white/10 via-white/5 to-gold/5 p-8 sm:p-10 lg:p-12">
            <div className="absolute inset-y-0 right-0 hidden w-1/2 bg-[radial-gradient(circle_at_center,rgba(151,212,255,0.16),transparent_60%)] lg:block" />
            <div className="relative grid gap-8 lg:grid-cols-[1.1fr_0.9fr] lg:items-center">
              <div>
                <span className="gold-label">
                  <WandSparkles className="size-4" />
                  Feito para encantar
                </span>
                <h2 className="mt-6 max-w-2xl font-display text-4xl text-white sm:text-5xl">Resultados bonitos, linguagem memorável e uma experiência que pede para ser compartilhada.</h2>
                <p className="mt-5 max-w-2xl text-base leading-8 text-mist/85 sm:text-lg">Da primeira impressão ao resultado final, cada detalhe do Oranum foi pensado para transmitir mistério, valor percebido e desejo imediato de explorar mais uma camada.</p>
              </div>
              <div className="grid gap-4 sm:grid-cols-2">
                {[
                  ["Nome", "Numerologia, energia, arquétipo e leitura simbólica"],
                  ["Data", "Signo solar, caminho de vida, potenciais e desafios"],
                  ["Vínculo", "Compatibilidade energética com texto relacional"],
                  ["Forma", "Visual premium, microinterações e experiência fluida"],
                ].map(([title, text]) => (
                  <div key={title} className="rounded-[24px] border border-white/10 bg-ink/60 p-5">
                    <p className="text-xs uppercase tracking-[0.32em] text-gold">{title}</p>
                    <p className="mt-3 text-sm leading-7 text-mist/85">{text}</p>
                  </div>
                ))}
              </div>
            </div>
          </MysticalCard>
        </div>
      </section>

      <TestimonialSection />
      <FAQSection />

      <section className="section-space pt-8">
        <div className="container-shell">
          <MysticalCard className="text-center">
            <div className="mx-auto max-w-3xl">
              <span className="gold-label">
                <Orbit className="size-4" />
                Último convite
              </span>
              <h2 className="mt-6 font-display text-4xl text-white sm:text-5xl">Revele agora a vibração que seu nome projeta no mundo.</h2>
              <p className="mt-4 text-base leading-8 text-mist/85 sm:text-lg">Em poucos instantes, o Oranum traduz símbolos em uma leitura elegante, envolvente e feita para tocar a curiosidade profunda de quem você é.</p>
              <div className="mt-8 flex justify-center">
                <a href="#topo" className="inline-flex items-center gap-2 rounded-full border border-gold/20 bg-gold/10 px-6 py-3 text-sm font-semibold uppercase tracking-[0.24em] text-gold transition hover:bg-gold/15">
                  <Sparkles className="size-4" />
                  Voltar ao ritual
                </a>
              </div>
            </div>
          </MysticalCard>
        </div>
      </section>
    </>
  );
}