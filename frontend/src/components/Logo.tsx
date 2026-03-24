import { MoonStar } from "lucide-react";
import { Link } from "react-router-dom";

export function Logo() {
  return (
    <Link to="/" className="inline-flex items-center gap-3 text-moon transition hover:text-white">
      <span className="flex size-11 items-center justify-center rounded-full border border-gold/20 bg-white/5 shadow-glow">
        <MoonStar className="size-5 text-gold" />
      </span>
      <span>
        <span className="block font-display text-3xl tracking-[0.2em] text-white">Oranum</span>
        <span className="block text-[0.68rem] uppercase tracking-[0.34em] text-mist/80">Seu nome revela a sua vibracao</span>
      </span>
    </Link>
  );
}
