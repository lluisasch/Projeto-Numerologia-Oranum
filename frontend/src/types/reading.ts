export type NameReadingRequest = {
  fullName: string;
};

export type BirthDateReadingRequest = {
  fullName: string;
  birthDate: string;
};

export type CompatibilityReadingRequest = {
  person1Name: string;
  person1BirthDate: string | null;
  person2Name: string;
  person2BirthDate: string | null;
};

export type NameReadingResponse = {
  nomeAnalisado: string;
  numeroPrincipal: number;
  tituloLeitura: string;
  energiaGeral: string;
  arquetipoPredominante: string;
  significadoDoNome: string;
  forcas: string[];
  desafios: string[];
  leituraXamanica: string;
  conselhoEspiritual: string;
  resumoFinal: string;
};

export type BirthDateReadingResponse = {
  dataNascimento: string;
  signoSolar: string;
  elemento: string;
  caminhoDeVida: number;
  energiaCentral: string;
  tendenciasEmocionais: string;
  missaoDeVida: string;
  desafios: string[];
  potenciais: string[];
  conselhoFinal: string;
};

export type CompatibilityReadingResponse = {
  pessoa1: string;
  pessoa2: string;
  compatibilidadePercentual: number;
  nivelCompatibilidade: string;
  afinidadeEnergetica: string;
  afinidadeEmocional: string;
  afinidadeEspiritual: string;
  pontosFortes: string[];
  pontosDeAtencao: string[];
  conselhoRelacional: string;
  resumoVinculo: string;
};
