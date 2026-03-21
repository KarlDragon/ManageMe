// Shared API helpers for front-end requests.
// This file centralizes the base URL and common fetch logic.

const API_BASE = import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5169/api";

export type ApiFetchOptions = Omit<RequestInit, "body"> & {
  body?: unknown;
};

export async function apiFetch<T>(path: string, options: ApiFetchOptions = {}): Promise<T> {
  const normalizedPath = path.startsWith("/") ? path : `/${path}`;
  const url = `${API_BASE}${normalizedPath}`;

  const init: RequestInit = {
    credentials: "include",
    headers: {
      "Content-Type": "application/json",
      ...(options.headers ?? {}),
    },
    ...options,
  } as RequestInit;

  if (options.body !== undefined) {
    // `RequestInit.body` expects a BodyInit type; using `any` here allows passing in JSON-friendly payloads.
    (init as any).body = JSON.stringify(options.body);
  }

  const response = await fetch(url, init);

  if (!response.ok) {
    const text = await response.text().catch(() => "");
    const message = text || response.statusText || "Unknown error";
    throw new Error(`API request failed (${response.status}): ${message}`);
  }

  // Some endpoints (e.g. logout) may return no JSON.
  if (response.status === 204) {
    return undefined as unknown as T;
  }

  return response.json();
}
