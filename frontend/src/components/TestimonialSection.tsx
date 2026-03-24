import { motion } from "framer-motion";
import { MysticalCard } from "@/components/MysticalCard";
import { SectionHeading } from "@/components/SectionHeading";

const testimonials = [
  {
    name: "Camila A.",
    role: "Diretora criativa",
    quote: "A leitura ficou elegante, sensível e com a sensação de que foi feita para mim. Compartilhei no mesmo instante.",
  },
  {
    name: "Renato M.",
    role: "Empreendedor",
    quote: "A mistura entre numerologia, arquétipos e linguagem mística trouxe profundidade sem soar exagerada. É envolvente do início ao fim.",
  },
  {
    name: "Lívia S.",
    role: "Consultora de imagem",
    quote: "O visual e o ritual da experiência fazem o resultado parecer um presente premium. Muito acima do comum.",
  },
];

export function TestimonialSection() {
  return (
    <section className="section-space">
      <div className="container-shell space-y-10">
        <SectionHeading
          eyebrow="Ecos de quem sentiu"
          title="Uma experiência feita para tocar imaginário, identidade e desejo de compartilhar"
          description="Cada leitura entrega impacto visual e emocional, com linguagem elegante e memorável desde o primeiro toque."
          align="center"
        />
        <div className="grid gap-6 lg:grid-cols-3">
          {testimonials.map((testimonial, index) => (
            <motion.div
              key={testimonial.name}
              initial={{ opacity: 0, y: 24 }}
              whileInView={{ opacity: 1, y: 0 }}
              viewport={{ once: true, amount: 0.25 }}
              transition={{ duration: 0.55, delay: index * 0.08 }}
            >
              <MysticalCard className="h-full">
                <p className="font-display text-3xl text-gold">“</p>
                <p className="-mt-2 text-base leading-8 text-moon/90">{testimonial.quote}</p>
                <div className="mt-8 border-t border-white/10 pt-5 text-sm text-mist/80">
                  <p className="font-semibold text-white">{testimonial.name}</p>
                  <p>{testimonial.role}</p>
                </div>
              </MysticalCard>
            </motion.div>
          ))}
        </div>
      </div>
    </section>
  );
}