import { Sparkles } from "lucide-react";
import { MysticalCard } from "@/components/MysticalCard";

type ArchetypeCardProps = {
  title: string;
  archetypeSummary: string;
  description: string;
  energyNote: string;
};

export function ArchetypeCard({ title, archetypeSummary, description, energyNote }: ArchetypeCardProps) {
  return (
    <MysticalCard className="h-full border-gold/10 bg-gradient-to-br from-white/10 to-white/5">
      <div className="flex items-center gap-3 text-gold">
        <Sparkles className="size-5" />
        <span className="text-xs uppercase tracking-[0.32em]">Arquétipo predominante</span>
      </div>
      <h3 className="mt-5 font-display text-3xl text-white">{title}</h3>
      <p className="mt-4 text-base font-medium text-moon">{archetypeSummary}</p>
      <p className="mt-3 text-sm leading-7 text-mist/85 sm:text-base">{description}</p>
      <div className="mt-5 rounded-[20px] border border-white/10 bg-white/5 p-4">
        <p className="text-xs uppercase tracking-[0.28em] text-gold/90">Como ele aparece na sua leitura</p>
        <p className="mt-3 text-sm leading-7 text-mist/85 sm:text-base">{energyNote}</p>
      </div>
    </MysticalCard>
  );
}