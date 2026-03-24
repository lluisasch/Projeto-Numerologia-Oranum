export type ArchetypeDetails = {
  name: string;
  summary: string;
  description: string;
};

const archetypeCatalog: Record<string, ArchetypeDetails> = {
  "Herói": {
    name: "Herói",
    summary: "Coragem, iniciativa e presença.",
    description: "Este arquétipo fala de quem sente um chamado para agir, abrir caminho e enfrentar desafios com firmeza.",
  },
  "Cuidador": {
    name: "Cuidador",
    summary: "Acolhimento, proteção e vínculo.",
    description: "Este arquétipo representa sensibilidade relacional, desejo de cuidar e capacidade de sustentar quem importa com delicadeza.",
  },
  "Criador": {
    name: "Criador",
    summary: "Imaginação, expressão e beleza.",
    description: "Este arquétipo revela uma natureza que gosta de transformar ideias, emoções e símbolos em algo único e memorável.",
  },
  "Governante": {
    name: "Governante",
    summary: "Estrutura, responsabilidade e direção.",
    description: "Este arquétipo representa ordem interior, senso de responsabilidade e talento para organizar a vida com clareza.",
  },
  "Explorador": {
    name: "Explorador",
    summary: "Liberdade, curiosidade e movimento.",
    description: "Este arquétipo fala de quem cresce quando atravessa novas experiências e precisa sentir espaço para descobrir o próprio caminho.",
  },
  "Amante": {
    name: "Amante",
    summary: "Afeto, conexão e intensidade.",
    description: "Este arquétipo representa o desejo de viver com o coração desperto, valorizando vínculos, beleza e trocas profundas.",
  },
  "Sábio": {
    name: "Sábio",
    summary: "Reflexão, discernimento e profundidade.",
    description: "Este arquétipo revela alguém que busca compreender antes de agir, encontrando força no olhar atento e na leitura do invisível.",
  },
  "Mago": {
    name: "Mago",
    summary: "Transformação, visão e simbolismo.",
    description: "Este arquétipo representa a capacidade de perceber possibilidades ocultas, conectar sentidos e transformar experiências em consciência.",
  },
  "Inocente": {
    name: "Inocente",
    summary: "Leveza, esperança e pureza de intenção.",
    description: "Este arquétipo fala de uma energia luminosa, sensível e confiante, que busca beleza, verdade e renovação.",
  },
  "Rebelde": {
    name: "Rebelde",
    summary: "Autenticidade, ruptura e coragem de mudar.",
    description: "Este arquétipo representa a força de romper padrões, questionar excessos e buscar uma vida mais alinhada com a própria essência.",
  },
};

export function getArchetypeDetails(name?: string | null): ArchetypeDetails {
  if (!name) {
    return {
      name: "Arquétipo",
      summary: "Presença simbólica em destaque.",
      description: "O arquétipo mostra a força principal que colore a sua leitura e ajuda a traduzir a energia do nome em linguagem humana.",
    };
  }

  return archetypeCatalog[name] ?? {
    name,
    summary: "Presença simbólica em destaque.",
    description: "O arquétipo mostra a força principal que colore a sua leitura e ajuda a traduzir a energia do nome em linguagem humana.",
  };
}