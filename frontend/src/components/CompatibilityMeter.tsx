import { motion } from "framer-motion";

type CompatibilityMeterProps = {
  value: number;
};

export function CompatibilityMeter({ value }: CompatibilityMeterProps) {
  const radius = 72;
  const circumference = 2 * Math.PI * radius;
  const progress = circumference - (Math.max(0, Math.min(100, value)) / 100) * circumference;

  return (
    <div className="relative flex items-center justify-center">
      <svg viewBox="0 0 180 180" className="size-52 drop-shadow-[0_0_30px_rgba(229,201,143,0.18)]">
        <circle cx="90" cy="90" r={radius} stroke="rgba(255,255,255,0.08)" strokeWidth="12" fill="none" />
        <motion.circle
          cx="90"
          cy="90"
          r={radius}
          stroke="url(#compatibility-gradient)"
          strokeWidth="12"
          fill="none"
          strokeLinecap="round"
          strokeDasharray={circumference}
          initial={{ strokeDashoffset: circumference }}
          animate={{ strokeDashoffset: progress }}
          transition={{ duration: 1.2, ease: "easeOut" }}
          transform="rotate(-90 90 90)"
        />
        <defs>
          <linearGradient id="compatibility-gradient" x1="0%" y1="0%" x2="100%" y2="100%">
            <stop offset="0%" stopColor="#97d4ff" />
            <stop offset="55%" stopColor="#c6b9dd" />
            <stop offset="100%" stopColor="#e5c98f" />
          </linearGradient>
        </defs>
      </svg>
      <div className="absolute inset-0 flex flex-col items-center justify-center">
        <span className="text-xs uppercase tracking-[0.3em] text-gold">Sintonia</span>
        <strong className="font-display text-6xl text-white">{value}</strong>
        <span className="text-sm text-mist/80">%</span>
      </div>
    </div>
  );
}
