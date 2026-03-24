import { useMemo, useState } from "react";
import { ArrowRight, CheckCircle2, Copy, LockKeyhole, QrCode, Sparkles } from "lucide-react";
import { MysticalCard } from "@/components/MysticalCard";
import { useToast } from "@/providers/ToastProvider";

type PremiumUnlockCardProps = {
  eyebrow: string;
  title: string;
  description: string;
  benefits: string[];
  pixCode: string;
  pixLabel: string;
  onTestUnlock: () => void;
  testButtonLabel: string;
};

export function PremiumUnlockCard({
  eyebrow,
  title,
  description,
  benefits,
  pixCode,
  pixLabel,
  onTestUnlock,
  testButtonLabel,
}: PremiumUnlockCardProps) {
  const { pushToast } = useToast();
  const [showPix, setShowPix] = useState(false);
  const pixKey = useMemo(() => pixCode.replaceAll(" ", ""), [pixCode]);

  const copyPixKey = async () => {
    try {
      await navigator.clipboard.writeText(pixKey);
      pushToast({ tone: "success", title: "Chave PIX copiada", description: "Use essa chave para simular o pagamento desta oferta." });
    } catch {
      pushToast({ tone: "info", title: "Copie manualmente", description: pixKey });
    }
  };

  return (
    <div className="space-y-6">
      <MysticalCard className="overflow-hidden border-gold/15 bg-gradient-to-br from-white/10 via-white/5 to-gold/10">
        <div className="flex flex-wrap items-start justify-between gap-4">
          <div>
            <p className="gold-label">
              <LockKeyhole className="size-4" />
              {eyebrow}
            </p>
            <h1 className="mt-6 max-w-2xl font-display text-4xl text-white sm:text-5xl">{title}</h1>
            <p className="mt-4 max-w-2xl text-base leading-8 text-mist/85">{description}</p>
          </div>
          <div className="rounded-[26px] border border-gold/20 bg-ink/70 p-5 text-right shadow-glow">
            <p className="text-xs uppercase tracking-[0.32em] text-gold/90">Promoção de abertura</p>
            <p className="mt-4 text-sm text-mist/60 line-through">De R$ 30,00</p>
            <p className="mt-1 font-display text-5xl text-white">R$ 14,99</p>
            <p className="mt-2 text-sm text-mist/75">Pagamento único por leitura</p>
          </div>
        </div>

        <div className="mt-8 grid gap-4 lg:grid-cols-3">
          {benefits.map((benefit) => (
            <div key={benefit} className="rounded-[22px] border border-white/10 bg-white/5 p-4 text-sm leading-7 text-mist/85">
              {benefit}
            </div>
          ))}
        </div>

        <div className="mt-8 grid gap-4 sm:grid-cols-2">
          <button
            type="button"
            onClick={() => setShowPix((current) => !current)}
            className="inline-flex h-14 items-center justify-center gap-2 rounded-2xl bg-gradient-to-r from-gold via-[#f6ddb1] to-gold px-6 font-semibold text-ink transition hover:brightness-105"
          >
            <QrCode className="size-4" />
            {showPix ? "Ocultar PIX" : "Pagar com PIX"}
          </button>
          <button
            type="button"
            onClick={onTestUnlock}
            className="inline-flex h-14 items-center justify-center gap-2 rounded-2xl border border-white/10 bg-white/5 px-6 font-semibold text-white transition hover:border-gold/20 hover:bg-white/10"
          >
            <Sparkles className="size-4 text-gold" />
            {testButtonLabel}
          </button>
        </div>

        <p className="mt-4 text-xs leading-6 text-mist/70">A primeira leitura do nome continua gratuita. As próximas camadas estão em oferta por R$ 14,99 cada nesta versão.</p>
      </MysticalCard>

      {showPix ? (
        <MysticalCard className="border-gold/10 bg-gradient-to-br from-white/10 to-white/5">
          <div className="grid gap-6 lg:grid-cols-[0.9fr_1.1fr] lg:items-center">
            <div className="mx-auto flex w-full max-w-[15rem] flex-col items-center rounded-[28px] border border-white/10 bg-ink/70 p-5">
              <div className="grid size-48 grid-cols-5 gap-1 rounded-[22px] border border-gold/20 bg-white p-4 shadow-glow">
                {Array.from({ length: 25 }).map((_, index) => (
                  <div
                    key={index}
                    className={index % 2 === 0 || index % 7 === 0 || index % 11 === 0 ? "rounded-sm bg-ink" : "rounded-sm bg-gold/80"}
                  />
                ))}
              </div>
              <p className="mt-4 text-center text-xs uppercase tracking-[0.3em] text-gold">{pixLabel}</p>
            </div>
            <div>
              <p className="text-sm uppercase tracking-[0.32em] text-gold">PIX promocional</p>
              <h2 className="mt-4 font-display text-3xl text-white">R$ 14,99 para liberar esta leitura</h2>
              <p className="mt-3 text-sm leading-7 text-mist/85 sm:text-base">Como esta fase ainda é de teste, o PIX funciona como uma experiência de checkout visual. Você pode copiar a chave abaixo ou seguir direto com o botão de teste.</p>
              <div className="mt-5 rounded-[22px] border border-white/10 bg-white/5 p-4">
                <p className="text-xs uppercase tracking-[0.28em] text-gold">Chave PIX</p>
                <p className="mt-3 break-all text-sm leading-7 text-moon">{pixKey}</p>
              </div>
              <div className="mt-5 flex flex-wrap gap-3">
                <button
                  type="button"
                  onClick={copyPixKey}
                  className="inline-flex h-12 items-center justify-center gap-2 rounded-2xl border border-white/10 bg-white/5 px-5 text-sm font-semibold text-white transition hover:border-gold/20 hover:bg-white/10"
                >
                  <Copy className="size-4 text-gold" />
                  Copiar chave PIX
                </button>
                <button
                  type="button"
                  onClick={onTestUnlock}
                  className="inline-flex h-12 items-center justify-center gap-2 rounded-2xl border border-gold/20 bg-gold/10 px-5 text-sm font-semibold text-gold transition hover:bg-gold/15"
                >
                  <CheckCircle2 className="size-4" />
                  Liberar em modo teste
                </button>
              </div>
            </div>
          </div>
        </MysticalCard>
      ) : null}

      <MysticalCard>
        <div className="flex items-center gap-3 text-gold">
          <ArrowRight className="size-5" />
          <span className="text-xs uppercase tracking-[0.32em]">Como funciona</span>
        </div>
        <div className="mt-5 space-y-3 text-sm leading-7 text-mist/85 sm:text-base">
          <p>1. A primeira leitura do nome fica liberada gratuitamente.</p>
          <p>2. Data de nascimento e compatibilidade entram como camadas premium em oferta.</p>
          <p>3. No teste, você pode pular o PIX e liberar a experiência com um clique.</p>
        </div>
      </MysticalCard>
    </div>
  );
}