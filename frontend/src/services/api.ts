import axios from "axios";

export const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? "http://localhost:8080/api",
  timeout: 45000,
});

api.interceptors.response.use(
  (response) => response,
  (error) => {
    const message =
      error.response?.data?.message ??
      error.message ??
      "Não foi possível consultar o oráculo agora.";

    return Promise.reject(new Error(message));
  },
);