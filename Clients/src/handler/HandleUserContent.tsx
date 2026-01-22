import type { UserContent } from "../models/UserContent";

export async function HandleUserContent(
  currentCategory: UserContent["category"],
  moneyAmount: number,
  note: string
) {
  const time_now = new Date();

  const payload = {
    Category: currentCategory,                  // match DTO names
    Amount: Math.round(moneyAmount),            // ensure integer for int32
    Note: note,
    DateIso: time_now.toISOString(),
    TzOffsetMinutes: time_now.getTimezoneOffset()
  };

  console.log("Payload to send:", payload);

  const response = await fetch("http://localhost:5169/api/UserContent/addusercontent", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(payload),
  });

  if (!response.ok) {
    const text = await response.text().catch(() => "");
    throw new Error(`HTTP ${response.status} ${text}`);
  }

  return response.json();
}