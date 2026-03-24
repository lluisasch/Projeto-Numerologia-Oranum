import { motion } from "framer-motion";
import { ArrowRight, Compass, MoonStar, Sparkles, Swords } from "lucide-react";
import { Link, useLocation } from "react-router-dom";
import { ArchetypeCard } from "@/components/ArchetypeCard";
import { MysticalCard } from "@/components/MysticalCard";
import { NumerologyBadge } from "@/components/NumerologyBadge";
import { usePageMeta } from "@/hooks/usePageMeta";
import type { NameReadingResponse } from "@/types/reading";
import { getArchetypeDetails } from "@/utils/archetypes";
import { storage } from "@/utils/storage";

export function NameResultPage() {
  const location = useLocation();
  const reading = ((location.state as { reading?: NameReadingResponse } | null)?.reading ?? storage.loadNameReading()) as NameReadingResponse | null;
  const archetype = getArchetypeDetails(reading?.arquetipoPredominante);

  usePageMeta(
    reading ? `${reading.nomeAnalisado} | Mapa energético Oranum` : "Resultado | Oranum",
    "Explore a energia do nome, o arquétipo predominante e os significados simbólicos revelados pelo Oranum.",
  );

  if (!reading) {
    return (
      <section className="container-shell section-space">
        <MysticalCard className="mx-auto max-w-2xl text-center">
          <p className="gold-label mx-auto">Mapa indisponível</p>
          <h1 className="mt-6 font-display text-4xl text-white">Seu resultado ainda não foi revelado.</h1>
          <p className="mt-4 text-base leading-8 text-mist/85">Volte para a página inicial, digite seu nome e inicie o ritual novamente.</p>
          <Link to="/" className="mt-8 inline-flex items-center gap-2 rounded-full border border-gold/20 bg-gold/10 px-6 py-3 text-sm font-semibold uppercase tracking-[0.24em] text-gold transition hover:bg-gold/15">
            <ArrowRight className="size-4" />
            Ir para o início
          </Link>
        </MysticalCard>
      </section>
    );
  }

  return (
    <section className="section-space">
      <div className="container-shell space-y-8">
        <motion.div initial={{ opacity: 0, y: 18 }} animate={{ opacity: 1, y: 0 }} className="space-y-5">
          <p className="gold-label">Leitura revelada</p>
          <h1 className="font-display text-5xl text-white sm:text-6xl">{reading.tituloLeitura}</h1>
          <p className="max-w-3xl text-lg leading-8 text-mist/85">{reading.resumoFinal}</p>
          <div className="flex flex-wrap items-center gap-4">
            <NumerologyBadge number={reading.numeroPrincipal} />
            <div className="rounded-full border border-gold/20 bg-gold/10 px-4 py-2 text-sm font-semibold text-gold">
              Primeira leitura gratuita
            </div>
          </div>
        </motion.div>

        <div className="grid gap-6 xl:grid-cols-[1.05fr_0.95fr] xl:items-start">
          <div className="space-y-6">
            <MysticalCard className="space-y-6">
              <div>
                <p className="text-sm uppercase tracking-[0.3em] text-gold">Energia geral</p>
                <p className="mt-3 text-sm leading-7 text-mist/90 sm:text-base">{reading.energiaGeral}</p>
              </div>
              <div>
                <p className="text-sm uppercase tracking-[0.3em] text-gold">Significado simbólico</p>
                <p className="mt-3 text-sm leading-7 text-mist/90 sm:text-base">{reading.significadoDoNome}</p>
              </div>
            </MysticalCard>

            <MysticalCard>
              <p className="text-sm uppercase tracking-[0.3em] text-gold">Leitura xamânica</p>
              <p className="mt-3 text-sm leading-7 text-mist/90 sm:text-base">{reading.leituraXamanica}</p>
            </MysticalCard>
          </div>

          <div className="space-y-6">
            <ArchetypeCard
              title={archetype.name}
              archetypeSummary={archetype.summary}
              description={archetype.description}
              energyNote={reading.energiaGeral}
            />
            <MysticalCard>
              <p className="text-sm uppercase tracking-[0.3em] text-gold">Conselho espiritual</p>
              <p className="mt-3 text-sm leading-7 text-moon sm:text-base">{reading.conselhoEspiritual}</p>
            </MysticalCard>
          </div>
        </div>

        <div className="grid gap-6 lg:grid-cols-2">
          <MysticalCard>
            <div className="flex items-center gap-3 text-gold">
              <MoonStar className="size-5" />
              <span className="text-xs uppercase tracking-[0.32em]">Forças</span>
            </div>
            <ul className="mt-5 space-y-3 text-sm leading-7 text-mist/85 sm:text-base">
              {reading.forcas.map((item) => (
                <li key={item}>• {item}</li>
              ))}
            </ul>
          </MysticalCard>

          <MysticalCard>
            <div className="flex items-center gap-3 text-gold">
              <Swords className="size-5" />
              <span className="text-xs uppercase tracking-[0.32em]">Desafios</span>
            </div>
            <ul className="mt-5 space-y-3 text-sm leading-7 text-mist/85 sm:text-base">
              {reading.desafios.map((item) => (
                <li key={item}>• {item}</li>
              ))}
            </ul>
          </MysticalCard>
        </div>

        <div className="space-y-4 pt-2">
          <div>
            <p className="gold-label">Continue sua leitura</p>
            <h2 className="mt-4 font-display text-3xl text-white sm:text-4xl">As próximas camadas entram com valor promocional.</h2>
            <p className="mt-3 max-w-3xl text-base leading-8 text-mist/85">Seu mapa do nome permanece gratuito. Para aprofundar a leitura pela data de nascimento ou abrir a compatibilidade, cada camada fica em oferta de R$ 30,00 por R$ 14,99.</p>
          </div>

          <div className="grid gap-6 lg:grid-cols-2">
            <Link to="/resultado/data" state={{ fullName: reading.nomeAnalisado }}>
              <MysticalCard className="h-full transition hover:border-gold/20 hover:bg-white/10">
                <div className="flex items-center justify-between gap-4">
                  <div className="flex items-center gap-3 text-gold">
                    <Compass className="size-5" />
                    <span className="text-xs uppercase tracking-[0.32em]">Próxima camada</span>
                  </div>
                  <div className="text-right">
                    <p className="text-xs text-mist/60 line-through">R$ 30,00</p>
                    <p className="text-sm font-semibold text-gold">R$ 14,99</p>
                  </div>
                </div>
                <h2 className="mt-5 font-display text-3xl text-white">Descobrir meu mapa pela data de nascimento</h2>
                <p className="mt-3 text-sm leading-7 text-mist/85 sm:text-base">Desbloqueie signo solar, caminho de vida, tendências emocionais, missão e potenciais.</p>
              </MysticalCard>
            </Link>

            <Link to="/resultado/compatibilidade" state={{ person1Name: reading.nomeAnalisado }}>
              <MysticalCard className="h-full transition hover:border-gold/20 hover:bg-white/10">
                <div className="flex items-center justify-between gap-4">
                  <div className="flex items-center gap-3 text-gold">
                    <Sparkles className="size-5" />
                    <span className="text-xs uppercase tracking-[0.32em]">Vínculos</span>
                  </div>
                  <div className="text-right">
                    <p className="text-xs text-mist/60 line-through">R$ 30,00</p>
                    <p className="text-sm font-semibold text-gold">R$ 14,99</p>
                  </div>
                </div>
                <h2 className="mt-5 font-display text-3xl text-white">Ver compatibilidade com outra pessoa</h2>
                <p className="mt-3 text-sm leading-7 text-mist/85 sm:text-base">Compare vibrações, afinidades e possíveis pontos de equilíbrio em uma leitura relacional mística.</p>
              </MysticalCard>
            </Link>
          </div>
        </div>
      </div>
    </section>
  );
}