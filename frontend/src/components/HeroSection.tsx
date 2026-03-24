import { motion } from "framer-motion";
import { Orbit, Sparkles, Stars } from "lucide-react";
import { NameInputForm } from "@/components/NameInputForm";

const highlights = [
  "Leitura do nome com numerologia e arquetipo predominante",
  "Camada complementar com signo solar, caminho de vida e missao simbolica",
  "Compatibilidade mistica com texto envolvente e compartilhavel",
];

export function HeroSection() {
  return (
    <section id="topo" className="relative overflow-hidden pb-20 pt-8 sm:pb-24 lg:pb-28 lg:pt-12">
      <div className="container-shell grid items-center gap-12 lg:grid-cols-[1.12fr_0.88fr]">
        <div>
          <motion.span initial={{ opacity: 0, y: 16 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.65 }} className="gold-label">
            <Stars className="size-4" />
            Experiencia mistica premium
          </motion.span>
          <motion.h1 initial={{ opacity: 0, y: 18 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.75, delay: 0.05 }} className="mt-6 max-w-3xl font-display text-5xl leading-[0.96] text-white sm:text-6xl lg:text-7xl">
            Seu nome guarda uma vibracao unica.
          </motion.h1>
          <motion.p initial={{ opacity: 0, y: 18 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.75, delay: 0.12 }} className="mt-6 max-w-2xl text-lg leading-8 text-mist/85 sm:text-xl">
            Descubra a energia, os arquetipos e os caminhos ocultos revelados pelo seu nome e pela sua data de nascimento.
          </motion.p>
          <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.75, delay: 0.18 }} className="mt-8">
            <NameInputForm />
          </motion.div>
          <div className="mt-8 grid gap-3 sm:grid-cols-3">
            {highlights.map((item, index) => (
              <motion.div key={item} initial={{ opacity: 0, y: 16 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.6, delay: 0.24 + index * 0.08 }} className="rounded-2xl border border-white/10 bg-white/5 px-4 py-4 text-sm leading-6 text-mist/85 backdrop-blur-xl">
                {item}
              </motion.div>
            ))}
          </div>
        </div>
        <motion.div initial={{ opacity: 0, scale: 0.94, y: 20 }} animate={{ opacity: 1, scale: 1, y: 0 }} transition={{ duration: 0.9, delay: 0.1 }} className="relative mx-auto flex w-full max-w-xl justify-center">
          <div className="relative aspect-[0.95] w-full rounded-[36px] border border-white/10 bg-gradient-to-br from-white/10 to-white/5 p-6 shadow-oracle backdrop-blur-xl">
            <div className="absolute inset-6 rounded-[28px] border border-gold/10 bg-nebula opacity-80" />
            <div className="relative flex h-full flex-col justify-between rounded-[28px] border border-white/10 bg-midnight/40 p-6">
              <div className="flex items-center justify-between text-mist/75">
                <span className="gold-label">Oranum</span>
                <Orbit className="size-5 text-gold" />
              </div>
              <div className="space-y-5 text-center">
                <div className="mx-auto flex size-28 animate-pulseGlow items-center justify-center rounded-full border border-gold/20 bg-gold/10 shadow-glow">
                  <Sparkles className="size-10 text-gold" />
                </div>
                <div>
                  <p className="text-sm uppercase tracking-[0.35em] text-mist/70">Mapa energetico do nome</p>
                  <p className="mt-4 font-display text-5xl text-white">Ritual de leitura</p>
                  <p className="mx-auto mt-4 max-w-sm text-sm leading-7 text-mist/80 sm:text-base">Um encontro entre numeros, simbolos e linguagem viva para transformar curiosidade em encantamento.</p>
                </div>
              </div>
              <div className="grid gap-3 sm:grid-cols-3">
                {[
                  ["Nome", "Essencia simbolica"],
                  ["Data", "Missao e tendencias"],
                  ["Compatibilidade", "Afinidade do vinculo"],
                ].map(([title, subtitle]) => (
                  <div key={title} className="rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-center">
                    <p className="text-xs uppercase tracking-[0.26em] text-gold/90">{title}</p>
                    <p className="mt-2 text-sm text-mist/80">{subtitle}</p>
                  </div>
                ))}
              </div>
            </div>
          </div>
        </motion.div>
      </div>
    </section>
  );
}
