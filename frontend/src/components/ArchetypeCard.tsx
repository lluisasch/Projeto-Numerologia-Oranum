import { Sparkles } from "lucide-react";
import { MysticalCard } from "@/components/MysticalCard";

type ArchetypeCardProps = {
  title: string;
  description: string;
};

export function ArchetypeCard({ title, description }: ArchetypeCardProps) {
  return (
    <MysticalCard className="h-full border-gold/10 bg-gradient-to-br from-white/10 to-white/5">
      <div className="flex items-center gap-3 text-gold">
        <Sparkles className="size-5" />
        <span className="text-xs uppercase tracking-[0.32em]">Arquetipo predominante</span>
      </div>
      <h3 className="mt-5 font-display text-3xl text-white">{title}</h3>
      <p className="mt-4 text-sm leading-7 text-mist/85 sm:text-base">{description}</p>
    </MysticalCard>
  );
}
