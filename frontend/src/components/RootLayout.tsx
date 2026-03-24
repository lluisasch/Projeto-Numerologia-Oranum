import { Outlet } from "react-router-dom";
import { CosmicBackground } from "@/components/CosmicBackground";
import { Footer } from "@/components/Footer";
import { Logo } from "@/components/Logo";

export function RootLayout() {
  return (
    <div className="relative min-h-screen overflow-hidden">
      <CosmicBackground />
      <div className="relative z-10">
        <header className="container-shell flex items-center justify-between py-6">
          <Logo />
          <div className="hidden rounded-full border border-white/10 bg-white/5 px-4 py-2 text-xs uppercase tracking-[0.28em] text-mist/75 sm:block">Conteudo interpretativo e ritualistico</div>
        </header>
        <main>
          <Outlet />
        </main>
        <Footer />
      </div>
    </div>
  );
}
