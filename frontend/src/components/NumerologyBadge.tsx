type NumerologyBadgeProps = {
  number: number;
};

export function NumerologyBadge({ number }: NumerologyBadgeProps) {
  return (
    <div className="inline-flex items-center gap-4 rounded-full border border-gold/20 bg-gold/10 px-5 py-3 shadow-glow">
      <span className="text-xs uppercase tracking-[0.32em] text-gold">Numero chave</span>
      <span className="font-display text-4xl text-white">{number}</span>
    </div>
  );
}
