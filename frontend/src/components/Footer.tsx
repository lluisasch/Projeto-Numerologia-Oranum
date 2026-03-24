import { Link } from "react-router-dom";
import { Logo } from "@/components/Logo";

export function Footer() {
  return (
    <footer className="border-t border-white/5 pb-12 pt-10">
      <div className="container-shell flex flex-col gap-8 lg:flex-row lg:items-end lg:justify-between">
        <div className="max-w-xl">
          <Logo />
          <p className="mt-4 text-sm leading-7 text-mist/80">Oranum transforma nome, símbolos e data de nascimento em uma experiência interpretativa desenhada para autoconhecimento, beleza e compartilhamento.</p>
        </div>
        <div className="space-y-3 text-sm text-mist/75 lg:text-right">
          <p>Conteúdo interpretativo para autoconhecimento e entretenimento.</p>
          <div className="flex flex-wrap gap-4 lg:justify-end">
            <Link to="/" className="transition hover:text-white">Início</Link>
            <a href="#faq" className="transition hover:text-white">FAQ</a>
            <a href="#como-funciona" className="transition hover:text-white">Como funciona</a>
          </div>
        </div>
      </div>
    </footer>
  );
}