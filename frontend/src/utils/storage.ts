import type {
  BirthDateReadingResponse,
  CompatibilityReadingResponse,
  NameReadingResponse,
} from "@/types/reading";

const keys = {
  nameReading: "oranum:name-reading",
  birthReading: "oranum:birth-reading",
  compatibilityReading: "oranum:compatibility-reading",
  lastName: "oranum:last-name",
  birthUnlocked: "oranum:birth-unlocked",
  compatibilityUnlocked: "oranum:compatibility-unlocked",
};

function save<T>(key: string, value: T) {
  localStorage.setItem(key, JSON.stringify(value));
}

function load<T>(key: string): T | null {
  const raw = localStorage.getItem(key);
  if (!raw) {
    return null;
  }

  try {
    return JSON.parse(raw) as T;
  } catch {
    return null;
  }
}

function loadBoolean(key: string) {
  return load<boolean>(key) ?? false;
}

export const storage = {
  saveNameReading(value: NameReadingResponse) {
    save(keys.nameReading, value);
    save(keys.lastName, value.nomeAnalisado);
  },
  loadNameReading() {
    return load<NameReadingResponse>(keys.nameReading);
  },
  saveBirthReading(value: BirthDateReadingResponse) {
    save(keys.birthReading, value);
  },
  loadBirthReading() {
    return load<BirthDateReadingResponse>(keys.birthReading);
  },
  saveCompatibilityReading(value: CompatibilityReadingResponse) {
    save(keys.compatibilityReading, value);
  },
  loadCompatibilityReading() {
    return load<CompatibilityReadingResponse>(keys.compatibilityReading);
  },
  saveLastName(fullName: string) {
    save(keys.lastName, fullName);
  },
  loadLastName() {
    return load<string>(keys.lastName);
  },
  unlockBirthAccess() {
    save(keys.birthUnlocked, true);
  },
  hasBirthAccess() {
    return loadBoolean(keys.birthUnlocked);
  },
  unlockCompatibilityAccess() {
    save(keys.compatibilityUnlocked, true);
  },
  hasCompatibilityAccess() {
    return loadBoolean(keys.compatibilityUnlocked);
  },
};