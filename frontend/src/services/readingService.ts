import { api } from "@/services/api";
import type {
  BirthDateReadingRequest,
  BirthDateReadingResponse,
  CompatibilityReadingRequest,
  CompatibilityReadingResponse,
  NameReadingRequest,
  NameReadingResponse,
} from "@/types/reading";

export const readingService = {
  async getNameReading(payload: NameReadingRequest) {
    const response = await api.post<NameReadingResponse>("/reading/name", payload);
    return response.data;
  },
  async getBirthDateReading(payload: BirthDateReadingRequest) {
    const response = await api.post<BirthDateReadingResponse>("/reading/birthdate", payload);
    return response.data;
  },
  async getCompatibilityReading(payload: CompatibilityReadingRequest) {
    const response = await api.post<CompatibilityReadingResponse>("/reading/compatibility", payload);
    return response.data;
  },
};
