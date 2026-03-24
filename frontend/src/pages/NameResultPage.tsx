import { motion } from "framer-motion";
import { ArrowRight, Compass, MoonStar, Sparkles, Swords } from "lucide-react";
import { Link, useLocation } from "react-router-dom";
import { ArchetypeCard } from "@/components/ArchetypeCard";
import { MysticalCard } from "@/components/MysticalCard";
import { NumerologyBadge } from "@/components/NumerologyBadge";
import { usePageMeta } from "@/hooks/usePageMeta";
import { storage } from "@/utils/storage";
import type { NameReadingResponse } from "@/types/reading";

export function NameResultPage() {
  const location = useLocation();
  const reading = ((location.state as { reading?: NameReadingResponse } | null)?.reading ?? storage.loadNameReading()) as NameReadingResponse | null;

  usePageMeta(
    reading ? `${reading.nomeAnalisado} | Mapa energetico Oranum` : "Resultado | Oranum",
    "Explore a energia do nome, seu arquetipo predominante e os significados simbolicos revelados pelo Oranum.",
  );

  if (!reading) {
    return (
      <section className="container-shell section-space">
        <MysticalCard className="mx-auto max-w-2xl text-center">
          <p className="gold-label mx-auto">Mapa indisponivel</p>
          <h1 className="mt-6 font-display text-4xl text-white">Seu resultado ainda nao foi revelado.</h1>
          <p className="mt-4 text-base leading-8 text-mist/85">Volte para a pagina inicial, digite seu nome e inicie o ritual novamente.</p>
          <Link to="/" className="mt-8 inline-flex items-center gap-2 rounded-full border border-gold/20 bg-gold/10 px-6 py-3 text-sm font-semibold uppercase tracking-[0.24em] text-gold transition hover:bg-gold/15">
            <ArrowRight className="size-4" />
            Ir para o inicio
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
          <NumerologyBadge number={reading.numeroPrincipal} />
        </motion.div>

        <div className="grid gap-6 lg:grid-cols-[1.15fr_0.85fr]">
          <MysticalCard className="space-y-6">
            <div>
              <p className="text-sm uppercase tracking-[0.3em] text-gold">Energia geral</p>
              <p className="mt-3 text-base leading-8 text-mist/90">{reading.energiaGeral}</p>
            </div>
            <div>
              <p className="text-sm uppercase tracking-[0.3em] text-gold">Significado simbolico</p>
              <p className="mt-3 text-base leading-8 text-mist/90">{reading.significadoDoNome}</p>
            </div>
            <div>
              <p className="text-sm uppercase tracking-[0.3em] text-gold">Leitura xamanica</p>
              <p className="mt-3 text-base leading-8 text-mist/90">{reading.leituraXamanica}</p>
            </div>
            <div className="rounded-[24px] border border-gold/10 bg-gold/5 p-5">
              <p className="text-sm uppercase tracking-[0.3em] text-gold">Conselho espiritual</p>
              <p className="mt-3 text-base leading-8 text-moon">{reading.conselhoEspiritual}</p>
            </div>
          </MysticalCard>

          <div className="space-y-6">
            <ArchetypeCard title={reading.arquetipoPredominante} description={reading.energiaGeral} />
            <MysticalCard>
              <div className="flex items-center gap-3 text-gold">
                <MoonStar className="size-5" />
                <span className="text-xs uppercase tracking-[0.32em]">Forcas</span>
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
        </div>

        <div className="grid gap-6 lg:grid-cols-2">
          <Link to="/resultado/data" state={{ fullName: reading.nomeAnalisado }}>
            <MysticalCard className="h-full transition hover:border-gold/20 hover:bg-white/10">
              <div className="flex items-center gap-3 text-gold">
                <Compass className="size-5" />
                <span className="text-xs uppercase tracking-[0.32em]">Proxima camada</span>
              </div>
              <h2 className="mt-5 font-display text-3xl text-white">Descobrir meu mapa pela data de nascimento</h2>
              <p className="mt-3 text-sm leading-7 text-mist/85 sm:text-base">Desbloqueie signo solar, caminho de vida, tendencias emocionais, missao e potenciais.</p>
            </MysticalCard>
          </Link>
          <Link to="/resultado/compatibilidade" state={{ person1Name: reading.nomeAnalisado }}>
            <MysticalCard className="h-full transition hover:border-gold/20 hover:bg-white/10">
              <div className="flex items-center gap-3 text-gold">
                <Sparkles className="size-5" />
                <span className="text-xs uppercase tracking-[0.32em]">Vinculos</span>
              </div>
              <h2 className="mt-5 font-display text-3xl text-white">Ver compatibilidade com outra pessoa</h2>
              <p className="mt-3 text-sm leading-7 text-mist/85 sm:text-base">Compare vibracoes, afinidades e possiveis pontos de equilibrio em uma leitura relacional mistica.</p>
            </MysticalCard>
          </Link>
        </div>
      </div>
    </section>
  );
}
