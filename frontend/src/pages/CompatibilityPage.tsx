import { zodResolver } from "@hookform/resolvers/zod";
import { HeartHandshake, Sparkles, WandSparkles } from "lucide-react";
import { useMemo, useState } from "react";
import { useForm } from "react-hook-form";
import { useLocation } from "react-router-dom";
import { z } from "zod";
import { CompatibilityMeter } from "@/components/CompatibilityMeter";
import { LoadingOracle } from "@/components/LoadingOracle";
import { MysticalCard } from "@/components/MysticalCard";
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

  usePageMeta("Compatibilidade energetica | Oranum", "Descubra afinidade energetica, emocional e espiritual entre dois nomes com a leitura de compatibilidade do Oranum.");

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
      pushToast({ tone: "success", title: "Compatibilidade revelada", description: "O vinculo entre os dois campos energeticos ja esta pronto para leitura." });
    } catch (error) {
      pushToast({ tone: "error", title: "Nao foi possivel abrir essa leitura relacional", description: error instanceof Error ? error.message : "Tente novamente em alguns instantes." });
    }
  });

  return (
    <section className="section-space">
      <div className="container-shell grid gap-8 lg:grid-cols-[0.82fr_1.18fr]">
        <div className="space-y-6">
          <MysticalCard>
            <p className="gold-label">Compatibilidade mistica</p>
            <h1 className="mt-6 font-display text-4xl text-white sm:text-5xl">Compare duas vibracoes e veja como esse encontro ressoa.</h1>
            <p className="mt-4 text-base leading-8 text-mist/85">O score nasce da numerologia dos nomes, da sintonia entre elementos e da camada narrativa criada para descrever o vinculo com clareza e misterio.</p>
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
                {isSubmitting ? "Entrelacando os mapas..." : "Revelar compatibilidade"}
                <HeartHandshake className="size-4" />
              </button>
            </form>
            <p className="mt-4 text-xs leading-6 text-mist/75">Voce pode informar apenas os nomes ou enriquecer a leitura com as datas de nascimento.</p>
          </MysticalCard>
        </div>

        <div>
          {isSubmitting ? (
            <LoadingOracle title="Entrelacando as vibracoes..." description="O Oranum esta comparando numeros, signos e simbolos para descrever a alquimia desse vinculo." />
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
                  <p className="text-sm uppercase tracking-[0.3em] text-gold">Pontos de atencao</p>
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
                <h2 className="mt-5 font-display text-4xl text-white">A compatibilidade aparecera aqui com porcentagem, afinidade e leitura do vinculo.</h2>
                <p className="mt-4 text-base leading-8 text-mist/85">Preencha os nomes ao lado para revelar o encontro energetico entre as duas pessoas.</p>
              </div>
            </MysticalCard>
          )}
        </div>
      </div>
    </section>
  );
}
