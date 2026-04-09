import type { SpendingData } from "../models/SpedingData";
import type { UserContent } from "../models/UserContent";
import { apiFetch } from "./api";

export function getSpendingData(hierarchy: "daily" | "monthly" | "yearly") {
  return apiFetch<SpendingData>(`/UserContent/spending/${hierarchy}`);
}

export function addUserContent(content: UserContent) {

  const { category, amount, note } = content;
  const now = new Date();

  return apiFetch<void>("/UserContent/addusercontent", {
    method: "POST",
    body: {
      Category: category,
      MoneySpent: Math.round(amount),
      Note: note,
      DateIso: now.toISOString(),
    },
  });
}
