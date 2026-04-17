import type { SpendingData } from "../models/SpedingData";
import type { UserContent } from "../models/UserContent";
import type { ModifiedContent } from "../models/ModifiedContent";
import { apiFetch } from "./api";

export function getSpendingData(hierarchy: "daily" | "monthly" | "yearly") {
  return apiFetch<SpendingData>(`/UserContent/spending/${hierarchy}`);
}

export function addUserContent(content: UserContent) {

  const { category, MoneySpent, note } = content;
  const now = new Date();

  return apiFetch<void>("/UserContent/addusercontent", {
    method: "POST",
    body: {
      Category: category,
      MoneySpent: Math.round(MoneySpent),
      Note: note,
      DateIso: now.toISOString(),
    },
  });
}

export function modifyUserContent(content: ModifiedContent) {
  const { id, category, MoneySpent, note } = content;     

  return apiFetch<void>("/UserContent/modifyusercontent", {
    method: "PUT",
    body: {
      Id: id,
      Category: category,
      MoneySpent: Math.round(MoneySpent),
      Note: note,
      DateIso: new Date().toISOString(),
    },
  });
}