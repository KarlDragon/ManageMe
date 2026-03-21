import type { LoginData, RegisterData } from "../models/Auth";
import type { AuthStatus } from "../models/AuthStatus";
import { apiFetch } from "./api";

export function getAuthStatus() {
  return apiFetch<AuthStatus>("/auth/status");
}

export function login({ email, password, rememberMe }: LoginData) {
  return apiFetch<AuthStatus>("/auth/login", {
    method: "POST",
    body: { email, password, rememberMe },
  });
}

export function register({ email, password }: RegisterData) {
  return apiFetch<AuthStatus>("/auth/register", {
    method: "POST",
    body: { email, password },
  });
}

export function logout() {
  return apiFetch<void>("/auth/logout", {
    method: "POST",
  });
}
