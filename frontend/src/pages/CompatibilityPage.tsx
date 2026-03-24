import { zodResolver } from "@hookform/resolvers/zod";
import { HeartHandshake, Sparkles, WandSparkles } from "lucide-react";
import { useMemo, useState } from "react";
import { useForm } from "react-hook-form";
import { useLocation } from "react-router-dom";
import { z } from "zod";
import { CompatibilityMeter } from "@/components/CompatibilityMeter";
import { LoadingOracle } from "@/components/LoadingOracle";
import { MysticalCard } from "@/components/MysticalCard";
import { PremiumUnlockCard } from "@/components/PremiumUnlockCard";
import { usePageMeta } from "@/hooks/usePageMeta";
import { useToast } from "@/providers/ToastProvider";
import { readingService } from "@/services/readingService";
import type { CompatibilityReadingResponse } from "@/types/reading";
import { storage } from "@/utils/storage";

const schema = z.object({
  person1Name: z.string().min(2, "Informe o primeiro nome."),
  person1BirthDate: z.string().optional(),
  person2Name: z.string().min(2, "Informe o segundo nome."),
  person2BirthDate: z.string().optional(),
});

type FormValues = z.infer<typeof schema>;

export function CompatibilityPage() {
  const location = useLocation();
  const state = (location.state as { person1Name?: string } | null) ?? null;
  const { pushToast } = useToast();
  const [result, setResult] = useState<CompatibilityReadingResponse | null>(storage.loadCompatibilityReading());
  const [hasAccess, setHasAccess] = useState(storage.hasCompatibilityAccess());

  usePageMeta("Compatibilidade energética | Oranum", "Descubra afinidade energética, emocional e espiritual entre dois nomes com a leitura de compatibilidade do Oranum.");

  const defaultPerson1 = useMemo(() => state?.person1Name ?? storage.loadLastName() ?? storage.loadNameReading()?.nomeAnalisado ?? "", [state]);

  const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: {
      person1Name: defaultPerson1,
      person1BirthDate: "",
      person2Name: "",
      person2BirthDate: "",
    },
  });

  const unlockTestAccess = () => {
    storage.unlockCompatibilityAccess();
    setHasAccess(true);
    pushToast({ tone: "success", title: "Compatibilidade liberada no modo teste", description: "Você já pode abrir a leitura relacional completa." });
  };

  const onSubmit = handleSubmit(async (values) => {
    try {
      const response = await readingService.getCompatibilityReading({
        person1Name: values.person1Name.trim(),
        person1BirthDate: values.person1BirthDate || null,
        person2Name: values.person2Name.trim(),
        person2BirthDate: values.person2BirthDate || null,
      });
      storage.saveCompatibilityReading(response);
      storage.saveLastName(values.person1Name.trim());
      setResult(response);
      pushToast({ tone: "success", title: "Compatibilidade revelada", description: "O vínculo entre os dois campos energéticos já está pronto para leitura." });
    } catch (error) {
      pushToast({ tone: "error", title: "Não foi possível abrir essa leitura relacional", description: error instanceof Error ? error.message : "Tente novamente em alguns instantes." });
    }
  });

  if (!hasAccess) {
    return (
      <section className="section-space">
        <div className="container-shell grid gap-8 lg:grid-cols-[1.1fr_0.9fr]">
          <PremiumUnlockCard
            eyebrow="Leitura premium"
            title="A compatibilidade fica liberada por um valor promocional."
            description="Depois da primeira leitura gratuita, você pode abrir a leitura relacional completa para comparar vibrações, afinidades e pontos de equilíbrio entre duas pessoas."
            benefits={[
              "Compatibilidade energética, emocional e espiritual em uma leitura só.",
              "Porcentagem, pontos fortes, pontos de atenção e conselho relacional.",
              "Oferta promocional de R$ 30,00 por R$ 14,99 nesta fase de teste.",
            ]}
            pixCode="oranum.compatibilidade@pix.teste"
            pixLabel="Compatibilidade"
            onTestUnlock={unlockTestAccess}
            testButtonLabel="Pular PIX e liberar no teste"
          />

          <div className="space-y-6">
            <MysticalCard>
              <p className="gold-label">Prévia do vínculo</p>
              <h2 className="mt-6 font-display text-4xl text-white">Uma leitura feita para entender a química entre dois mapas.</h2>
              <p className="mt-4 text-base leading-8 text-mist/85">Ao liberar a compatibilidade, o Oranum cruza nome, arquétipos e, se você quiser, as datas de nascimento para descrever o tipo de encontro que essa relação sugere.</p>
              <div className="mt-6 grid gap-4 sm:grid-cols-2">
                {[
                  ["Porcentagem", "Uma medida simbólica de sintonia entre as duas pessoas."],
                  ["Afinidades", "Leitura energética, emocional e espiritual em textos separados."],
                  ["Pontos fortes", "O que tende a sustentar e enriquecer o vínculo."],
                  ["Pontos de atenção", "Os temas que pedem mais consciência e cuidado."],
                ].map(([title, text]) => (
                  <div key={title} className="rounded-[22px] border border-white/10 bg-white/5 p-4">
                    <p className="text-xs uppercase tracking-[0.28em] text-gold">{title}</p>
                    <p className="mt-3 text-sm leading-7 text-mist/85">{text}</p>
                  </div>
                ))}
              </div>
            </MysticalCard>
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
            <p className="gold-label">Compatibilidade mística</p>
            <h1 className="mt-6 font-display text-4xl text-white sm:text-5xl">Compare duas vibrações e veja como esse encontro ressoa.</h1>
            <p className="mt-4 text-base leading-8 text-mist/85">A leitura nasce do encontro entre os ritmos dos nomes, dos arquétipos e, quando você quiser, das datas de nascimento para revelar afinidades, tensões e pontos de equilíbrio.</p>
            <form onSubmit={onSubmit} className="mt-8 space-y-4">
              <div className="grid gap-4 sm:grid-cols-2">
                <div>
                  <input {...register("person1Name")} placeholder="Nome da pessoa 1" className="h-14 w-full rounded-2xl border border-white/10 bg-ink/80 px-4 text-white outline-none transition placeholder:text-mist/55 focus:border-gold/30" />
                  {errors.person1Name ? <p className="mt-2 text-sm text-rose-200">{errors.person1Name.message}</p> : null}
                </div>
                <div>
                  <input {...register("person1BirthDate")} type="date" className="h-14 w-full rounded-2xl border border-white/10 bg-ink/80 px-4 text-white outline-none transition focus:border-gold/30" />
                </div>
              </div>
              <div className="grid gap-4 sm:grid-cols-2">
                <div>
                  <input {...register("person2Name")} placeholder="Nome da pessoa 2" className="h-14 w-full rounded-2xl border border-white/10 bg-ink/80 px-4 text-white outline-none transition placeholder:text-mist/55 focus:border-gold/30" />
                  {errors.person2Name ? <p className="mt-2 text-sm text-rose-200">{errors.person2Name.message}</p> : null}
                </div>
                <div>
                  <input {...register("person2BirthDate")} type="date" className="h-14 w-full rounded-2xl border border-white/10 bg-ink/80 px-4 text-white outline-none transition focus:border-gold/30" />
                </div>
              </div>
              <button type="submit" disabled={isSubmitting} className="inline-flex h-14 w-full items-center justify-center gap-2 rounded-2xl bg-gradient-to-r from-gold via-[#f6ddb1] to-gold px-6 font-semibold text-ink transition hover:brightness-105 disabled:cursor-not-allowed disabled:opacity-70">
                {isSubmitting ? "Entrelaçando os mapas..." : "Gerar compatibilidade"}
                <HeartHandshake className="size-4" />
              </button>
            </form>
            <p className="mt-4 text-xs leading-6 text-mist/75">Você pode informar apenas os nomes ou enriquecer a leitura com as datas de nascimento.</p>
          </MysticalCard>
        </div>

        <div>
          {isSubmitting ? (
            <LoadingOracle title="Entrelaçando as vibrações..." description="O Oranum está comparando números, signos e símbolos para descrever a alquimia desse vínculo." />
          ) : result ? (
            <div className="space-y-6">
              <MysticalCard className="overflow-hidden">
                <div className="grid gap-6 lg:grid-cols-[0.8fr_1.2fr] lg:items-center">
                  <CompatibilityMeter value={result.compatibilidadePercentual} />
                  <div>
                    <p className="gold-label">{result.nivelCompatibilidade}</p>
                    <h2 className="mt-5 font-display text-4xl text-white sm:text-5xl">{result.pessoa1} + {result.pessoa2}</h2>
                    <p className="mt-4 text-base leading-8 text-mist/90">{result.resumoVinculo}</p>
                  </div>
                </div>
              </MysticalCard>
              <div className="grid gap-6 lg:grid-cols-2">
                <MysticalCard>
                  <div className="flex items-center gap-3 text-gold">
                    <Sparkles className="size-5" />
                    <span className="text-xs uppercase tracking-[0.3em]">Afinidades</span>
                  </div>
                  <div className="mt-4 space-y-4 text-sm leading-7 text-mist/90 sm:text-base">
                    <p>{result.afinidadeEnergetica}</p>
                    <p>{result.afinidadeEmocional}</p>
                    <p>{result.afinidadeEspiritual}</p>
                  </div>
                </MysticalCard>
                <MysticalCard>
                  <div className="flex items-center gap-3 text-gold">
                    <WandSparkles className="size-5" />
                    <span className="text-xs uppercase tracking-[0.3em]">Conselho relacional</span>
                  </div>
                  <p className="mt-4 text-sm leading-7 text-mist/90 sm:text-base">{result.conselhoRelacional}</p>
                </MysticalCard>
              </div>
              <div className="grid gap-6 lg:grid-cols-2">
                <MysticalCard>
                  <p className="text-sm uppercase tracking-[0.3em] text-gold">Pontos fortes</p>
                  <ul className="mt-4 space-y-3 text-sm leading-7 text-mist/85 sm:text-base">
                    {result.pontosFortes.map((item) => (
                      <li key={item}>• {item}</li>
                    ))}
                  </ul>
                </MysticalCard>
                <MysticalCard>
                  <p className="text-sm uppercase tracking-[0.3em] text-gold">Pontos de atenção</p>
                  <ul className="mt-4 space-y-3 text-sm leading-7 text-mist/85 sm:text-base">
                    {result.pontosDeAtencao.map((item) => (
                      <li key={item}>• {item}</li>
                    ))}
                  </ul>
                </MysticalCard>
              </div>
            </div>
          ) : (
            <MysticalCard className="flex min-h-[24rem] items-center justify-center text-center">
              <div className="max-w-xl">
                <p className="gold-label mx-auto">Aguardando dois nomes</p>
                <h2 className="mt-5 font-display text-4xl text-white">A compatibilidade aparecerá aqui com porcentagem, afinidade e leitura do vínculo.</h2>
                <p className="mt-4 text-base leading-8 text-mist/85">Preencha os nomes ao lado para revelar o encontro energético entre as duas pessoas.</p>
              </div>
            </MysticalCard>
          )}
        </div>
      </div>
    </section>
  );
}