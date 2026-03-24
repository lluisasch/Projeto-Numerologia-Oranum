import { Link } from "react-router-dom";
import { MysticalCard } from "@/components/MysticalCard";
import { usePageMeta } from "@/hooks/usePageMeta";

export function NotFoundPage() {
  usePageMeta("404 | Oranum", "A pagina procurada nao foi encontrada no mapa do Oranum.");

  return (
    <section className="container-shell section-space">
      <MysticalCard className="mx-auto max-w-3xl text-center">
        <p className="gold-label mx-auto">404</p>
        <h1 className="mt-6 font-display text-5xl text-white sm:text-6xl">Este portal ainda nao foi aberto.</h1>
        <p className="mx-auto mt-4 max-w-2xl text-base leading-8 text-mist/85 sm:text-lg">O caminho que voce tentou seguir nao esta disponivel neste mapa. Retorne ao inicio e reabra o ritual pela entrada principal.</p>
        <Link to="/" className="mt-8 inline-flex items-center justify-center rounded-full border border-gold/20 bg-gold/10 px-6 py-3 text-sm font-semibold uppercase tracking-[0.24em] text-gold transition hover:bg-gold/15">
          Voltar ao inicio
        </Link>
      </MysticalCard>
    </section>
  );
}
