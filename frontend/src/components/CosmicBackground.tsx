import { motion } from "framer-motion";

const orbs = [
  "left-[8%] top-[12%] h-56 w-56 bg-violet-500/20",
  "right-[10%] top-[18%] h-72 w-72 bg-sky-400/10",
  "left-[30%] bottom-[20%] h-80 w-80 bg-fuchsia-500/10",
  "right-[18%] bottom-[10%] h-52 w-52 bg-amber-200/10",
];

export function CosmicBackground() {
  return (
    <div className="pointer-events-none fixed inset-0 overflow-hidden">
      {orbs.map((orb, index) => (
        <motion.div
          key={orb}
          className={`absolute rounded-full blur-3xl ${orb}`}
          animate={{ x: [0, index % 2 === 0 ? 30 : -24, 0], y: [0, index % 2 === 0 ? -18 : 20, 0], opacity: [0.25, 0.5, 0.28] }}
          transition={{ duration: 10 + index * 2, repeat: Number.POSITIVE_INFINITY, ease: "easeInOut" }}
        />
      ))}
      <div className="absolute inset-x-0 top-0 h-40 bg-gradient-to-b from-starlight/5 to-transparent" />
      <div className="absolute inset-x-0 bottom-0 h-60 bg-gradient-to-t from-midnight via-midnight/70 to-transparent" />
    </div>
  );
}
