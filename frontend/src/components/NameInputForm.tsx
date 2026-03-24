import { zodResolver } from "@hookform/resolvers/zod";
import { ArrowRight, Sparkles } from "lucide-react";
import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { z } from "zod";
import { useToast } from "@/providers/ToastProvider";
import { readingService } from "@/services/readingService";
import { storage } from "@/utils/storage";

const schema = z.object({
  fullName: z.string().min(2, "Digite seu nome completo ou o nome que deseja analisar."),
});

type FormValues = z.infer<typeof schema>;

export function NameInputForm() {
  const navigate = useNavigate();
  const { pushToast } = useToast();
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: {
      fullName: storage.loadLastName() ?? "",
    },
  });

  const onSubmit = handleSubmit(async (values) => {
    try {
      const response = await readingService.getNameReading({ fullName: values.fullName.trim() });
      storage.saveNameReading(response);
      pushToast({ tone: "success", title: "Mapa revelado", description: "Sua leitura já está pronta para ser explorada em detalhes." });
      navigate("/resultado/nome", { state: { reading: response } });
    } catch (error) {
      pushToast({
        tone: "error",
        title: "Não foi possível concluir a leitura",
        description: error instanceof Error ? error.message : "Tente novamente em alguns instantes.",
      });
    }
  });

  return (
    <form onSubmit={onSubmit} className="glass-panel w-full max-w-2xl p-4 sm:p-5">
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center">
        <label className="relative flex-1">
          <Sparkles className="pointer-events-none absolute left-4 top-1/2 size-5 -translate-y-1/2 text-gold" />
          <input
            {...register("fullName")}
            placeholder="Digite seu nome para revelar a sua vibração"
            className="h-14 w-full rounded-2xl border border-white/10 bg-ink/80 pl-12 pr-4 text-base text-white outline-none ring-0 transition placeholder:text-mist/55 focus:border-gold/30"
          />
        </label>
        <button type="submit" disabled={isSubmitting} className="inline-flex h-14 items-center justify-center gap-2 rounded-2xl bg-gradient-to-r from-gold via-[#f6ddb1] to-gold px-6 font-semibold text-ink transition hover:brightness-105 disabled:cursor-not-allowed disabled:opacity-70">
          {isSubmitting ? "Consultando energias..." : "Revelar meu mapa energético"}
          <ArrowRight className="size-4" />
        </button>
      </div>
      {errors.fullName ? <p className="mt-3 text-sm text-rose-200">{errors.fullName.message}</p> : null}
      <p className="mt-3 text-xs leading-6 text-mist/75">Conteúdo interpretativo para autoconhecimento e entretenimento.</p>
    </form>
  );
}