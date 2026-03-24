import { zodResolver } from "@hookform/resolvers/zod";
import { motion } from "framer-motion";
import { CalendarDays, Compass, Sparkles, Target } from "lucide-react";
import { useMemo, useState } from "react";
import { useForm } from "react-hook-form";
import { Link, useLocation } from "react-router-dom";
import { z } from "zod";
import { LoadingOracle } from "@/components/LoadingOracle";
import { MysticalCard } from "@/components/MysticalCard";
import { NumerologyBadge } from "@/components/NumerologyBadge";
import { PremiumUnlockCard } from "@/components/PremiumUnlockCard";
import { usePageMeta } from "@/hooks/usePageMeta";
import { useToast } from "@/providers/ToastProvider";
import { readingService } from "@/services/readingService";
import type { BirthDateReadingResponse } from "@/types/reading";
import { storage } from "@/utils/storage";

const schema = z.object({
  fullName: z.string().min(2, "Informe um nome válido."),
  birthDate: z.string().min(1, "Selecione a data de nascimento."),
});

type FormValues = z.infer<typeof schema>;

export function BirthDatePage() {
  const location = useLocation();
  const state = (location.state as { fullName?: string } | null) ?? null;
  const { pushToast } = useToast();
  const [result, setResult] = useState<BirthDateReadingResponse | null>(storage.loadBirthReading());
  const [hasAccess, setHasAccess] = useState(storage.hasBirthAccess());

  usePageMeta("Mapa da data de nascimento | Oranum", "Descubra signo solar, elemento, caminho de vida e uma leitura simbólica da sua data de nascimento com o Oranum.");

  const defaultFullName = useMemo(() => state?.fullName ?? storage.loadLastName() ?? storage.loadNameReading()?.nomeAnalisado ?? "", [state]);

  const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: {
      fullName: defaultFullName,
      birthDate: result?.dataNascimento ?? "",
    },
  });

  const unlockTestAccess = () => {
    storage.unlockBirthAccess();
    setHasAccess(true);
    pushToast({ tone: "success", title: "Acesso liberado no modo teste", description: "A leitura da data de nascimento já pode ser consultada." });
  };

  const onSubmit = handleSubmit(async (values) => {
    try {
      const response = await readingService.getBirthDateReading(values);
      storage.saveBirthReading(response);
      storage.saveLastName(values.fullName.trim());
      setResult(response);
      pushToast({ tone: "success", title: "Mapa complementar liberado", description: "Sua data de nascimento trouxe uma nova camada da leitura." });
    } catch (error) {
      pushToast({ tone: "error", title: "Não foi possível consultar essa data agora", description: error instanceof Error ? error.message : "Tente novamente em alguns instantes." });
    }
  });

  if (!hasAccess) {
    return (
      <section className="section-space">
        <div className="container-shell grid gap-8 lg:grid-cols-[1.1fr_0.9fr]">
          <PremiumUnlockCard
            eyebrow="Camada premium"
            title="Seu mapa pela data de nascimento entra com valor promocional."
            description="A leitura do nome segue gratuita. Para abrir a camada da data de nascimento, você desbloqueia signo solar, elemento, caminho de vida, missão e potenciais por um valor único de lançamento."
            benefits={[
              "Signo solar, elemento e caminho de vida em uma leitura complementar.",
              "Missão, potenciais e desafios recorrentes em linguagem mística e elegante.",
              "Oferta promocional de R$ 30,00 por R$ 14,99 nesta fase de abertura.",
            ]}
            pixCode="oranum.data@pix.teste"
            pixLabel="Mapa da data"
            onTestUnlock={unlockTestAccess}
            testButtonLabel="Pular PIX e liberar no teste"
          />

          <div className="space-y-6">
            <MysticalCard>
              <p className="gold-label">Prévia do que você recebe</p>
              <h2 className="mt-6 font-display text-4xl text-white">Uma segunda camada para aprofundar o seu mapa.</h2>
              <p className="mt-4 text-base leading-8 text-mist/85">Ao liberar essa leitura, o Oranum une o nome com a data de nascimento para revelar tendências emocionais, missão e potenciais com mais profundidade.</p>
              <div className="mt-6 grid gap-4 sm:grid-cols-2">
                {[
                  ["Signo solar", "Como sua identidade se expressa e ganha presença."],
                  ["Elemento", "O clima emocional que colore sua forma de sentir."],
                  ["Caminho de vida", "O número que aponta aprendizados e direções."],
                  ["Conselho final", "Um fechamento simbólico para orientar os próximos passos."],
                ].map(([title, text]) => (
                  <div key={title} className="rounded-[22px] border border-white/10 bg-white/5 p-4">
                    <p className="text-xs uppercase tracking-[0.28em] text-gold">{title}</p>
                    <p className="mt-3 text-sm leading-7 text-mist/85">{text}</p>
                  </div>
                ))}
              </div>
            </MysticalCard>

            <Link to="/resultado/compatibilidade" state={{ person1Name: defaultFullName }} className="block">
              <MysticalCard className="transition hover:border-gold/20 hover:bg-white/10">
                <p className="text-sm uppercase tracking-[0.3em] text-gold">Quer ir além depois?</p>
                <h2 className="mt-3 font-display text-3xl text-white">A compatibilidade também fica em oferta.</h2>
                <p className="mt-3 text-sm leading-7 text-mist/85">Depois desta camada, você ainda pode comparar seu mapa com outra pessoa por R$ 14,99.</p>
              </MysticalCard>
            </Link>
          </div>
        </div>
      </section>
    );
  }

  return (
    <section className="section-space">
      <div className="container-shell grid gap-8 lg:grid-cols-[0.82fr_1.18fr]">
        <div className="space-y-6">
          <MysticalCard>
            <p className="gold-label">Camada complementar</p>
            <h1 className="mt-6 font-display text-4xl text-white sm:text-5xl">Revele o mapa simbólico da sua data de nascimento.</h1>
            <p className="mt-4 text-base leading-8 text-mist/85">Aqui o Oranum conecta signo solar, elemento e caminho de vida para aprofundar a leitura iniciada pelo nome.</p>
            <form onSubmit={onSubmit} className="mt-8 space-y-4">
              <div>
                <input {...register("fullName")} placeholder="Seu nome" className="h-14 w-full rounded-2xl border border-white/10 bg-ink/80 px-4 text-white outline-none transition placeholder:text-mist/55 focus:border-gold/30" />
                {errors.fullName ? <p className="mt-2 text-sm text-rose-200">{errors.fullName.message}</p> : null}
              </div>
              <div>
                <input {...register("birthDate")} type="date" className="h-14 w-full rounded-2xl border border-white/10 bg-ink/80 px-4 text-white outline-none transition focus:border-gold/30" />
                {errors.birthDate ? <p className="mt-2 text-sm text-rose-200">{errors.birthDate.message}</p> : null}
              </div>
              <button type="submit" disabled={isSubmitting} className="inline-flex h-14 w-full items-center justify-center gap-2 rounded-2xl bg-gradient-to-r from-gold via-[#f6ddb1] to-gold px-6 font-semibold text-ink transition hover:brightness-105 disabled:cursor-not-allowed disabled:opacity-70">
                {isSubmitting ? "Consultando signos e caminhos..." : "Gerar leitura da data"}
                <CalendarDays className="size-4" />
              </button>
            </form>
            <p className="mt-4 text-xs leading-6 text-mist/75">Conteúdo interpretativo para autoconhecimento e entretenimento.</p>
          </MysticalCard>
          <Link to="/resultado/compatibilidade" state={{ person1Name: defaultFullName }} className="block">
            <MysticalCard className="transition hover:border-gold/20 hover:bg-white/10">
              <p className="text-sm uppercase tracking-[0.3em] text-gold">Pronto para a próxima leitura?</p>
              <h2 className="mt-3 font-display text-3xl text-white">Leve esse mapa para a compatibilidade.</h2>
              <p className="mt-3 text-sm leading-7 text-mist/85">Compare a vibração do seu nome com outra pessoa e revele o tipo de encontro que essa união sugere.</p>
              <p className="mt-4 text-sm font-semibold text-gold">Promoção ativa: de R$ 30,00 por R$ 14,99.</p>
            </MysticalCard>
          </Link>
        </div>

        <div>
          {isSubmitting ? (
            <LoadingOracle title="Lendo sua data de nascimento..." description="Entre signos, elementos e ciclos, o oráculo está montando sua síntese simbólica." />
          ) : result ? (
            <div className="space-y-6">
              <motion.div initial={{ opacity: 0, y: 18 }} animate={{ opacity: 1, y: 0 }} className="flex flex-wrap items-end gap-4">
                <div>
                  <p className="gold-label">Mapa da data</p>
                  <h2 className="mt-4 font-display text-5xl text-white">{result.signoSolar}</h2>
                  <p className="mt-2 text-base text-mist/85">Elemento {result.elemento}</p>
                </div>
                <NumerologyBadge number={result.caminhoDeVida} />
              </motion.div>
              <div className="grid gap-6 lg:grid-cols-2">
                <MysticalCard>
                  <div className="flex items-center gap-3 text-gold">
                    <Compass className="size-5" />
                    <span className="text-xs uppercase tracking-[0.3em]">Energia central</span>
                  </div>
                  <p className="mt-4 text-sm leading-7 text-mist/90 sm:text-base">{result.energiaCentral}</p>
                  <p className="mt-4 text-sm leading-7 text-mist/75 sm:text-base">{result.tendenciasEmocionais}</p>
                </MysticalCard>
                <MysticalCard>
                  <div className="flex items-center gap-3 text-gold">
                    <Target className="size-5" />
                    <span className="text-xs uppercase tracking-[0.3em]">Missão de vida</span>
                  </div>
                  <p className="mt-4 text-sm leading-7 text-mist/90 sm:text-base">{result.missaoDeVida}</p>
                  <div className="mt-5 rounded-[24px] border border-gold/10 bg-gold/5 p-4 text-sm leading-7 text-moon">{result.conselhoFinal}</div>
                </MysticalCard>
              </div>
              <div className="grid gap-6 lg:grid-cols-2">
                <MysticalCard>
                  <p className="text-sm uppercase tracking-[0.3em] text-gold">Desafios recorrentes</p>
                  <ul className="mt-4 space-y-3 text-sm leading-7 text-mist/85 sm:text-base">
                    {result.desafios.map((item) => (
                      <li key={item}>• {item}</li>
                    ))}
                  </ul>
                </MysticalCard>
                <MysticalCard>
                  <div className="flex items-center gap-3 text-gold">
                    <Sparkles className="size-5" />
                    <span className="text-xs uppercase tracking-[0.3em]">Potenciais</span>
                  </div>
                  <ul className="mt-4 space-y-3 text-sm leading-7 text-mist/85 sm:text-base">
                    {result.potenciais.map((item) => (
                      <li key={item}>• {item}</li>
                    ))}
                  </ul>
                </MysticalCard>
              </div>
            </div>
          ) : (
            <MysticalCard className="flex min-h-[24rem] items-center justify-center text-center">
              <div className="max-w-xl">
                <p className="gold-label mx-auto">Aguardando a data</p>
                <h2 className="mt-5 font-display text-4xl text-white">Seu signo, elemento e caminho de vida aparecem aqui.</h2>
                <p className="mt-4 text-base leading-8 text-mist/85">Preencha a data para desbloquear sua camada complementar com missão, potenciais e conselhos simbólicos.</p>
              </div>
            </MysticalCard>
          )}
        </div>
      </div>
    </section>
  );
}