import { motion } from "framer-motion";

type SectionHeadingProps = {
  eyebrow: string;
  title: string;
  description: string;
  align?: "left" | "center";
};

export function SectionHeading({ eyebrow, title, description, align = "left" }: SectionHeadingProps) {
  const alignment = align === "center" ? "text-center mx-auto" : "text-left";

  return (
    <motion.div
      initial={{ opacity: 0, y: 18 }}
      whileInView={{ opacity: 1, y: 0 }}
      viewport={{ once: true, amount: 0.35 }}
      transition={{ duration: 0.6 }}
      className={`max-w-3xl ${alignment}`}
    >
      <span className="gold-label">{eyebrow}</span>
      <h2 className="mt-5 font-display text-4xl text-white sm:text-5xl">{title}</h2>
      <p className="mt-4 text-base leading-8 text-mist/85 sm:text-lg">{description}</p>
    </motion.div>
  );
}
