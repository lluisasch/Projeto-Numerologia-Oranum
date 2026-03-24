import { motion } from "framer-motion";
import { Sparkles } from "lucide-react";

type LoadingOracleProps = {
  title?: string;
  description?: string;
};

export function LoadingOracle({ title = "Consultando o oraculo...", description = "Alinhando simbolos, numeros e sinais sutis para revelar sua leitura." }: LoadingOracleProps) {
  return (
    <div className="glass-panel flex flex-col items-center justify-center gap-5 px-6 py-12 text-center">
      <motion.div className="relative flex size-20 items-center justify-center rounded-full border border-gold/20 bg-gold/10" animate={{ rotate: 360 }} transition={{ duration: 14, repeat: Number.POSITIVE_INFINITY, ease: "linear" }}>
        <motion.div className="absolute inset-2 rounded-full border border-dashed border-starlight/30" animate={{ rotate: -360 }} transition={{ duration: 18, repeat: Number.POSITIVE_INFINITY, ease: "linear" }} />
        <Sparkles className="size-8 text-gold" />
      </motion.div>
      <div>
        <p className="font-display text-3xl text-white">{title}</p>
        <p className="mt-2 max-w-xl text-sm leading-7 text-mist/85 sm:text-base">{description}</p>
      </div>
    </div>
  );
}
