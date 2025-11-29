import type { UserContent } from "../models/UserContent";

export function HandleUserContent(currentCategory: UserContent["category"], moneyAmount: number, note: string) {
  console.log("Handling user content:", { currentCategory, moneyAmount, note });
  
  const time_now = new Date();
  console.log("Current time:", time_now.toString());

  const payload = {
    category: currentCategory,
    amount: Math.round(moneyAmount * 1000),  // Convert to cents or smallest unit
    note: note,
    dateIso: time_now.toISOString(),  // UTC time
    tzOffsetMinutes: time_now.getTimezoneOffset()  // e.g., -420 for UTC+7
  };

  console.log("Payload to send:", payload);

  fetch('http://localhost:5169/api/UserContent/addusercontent', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    credentials: 'include',
    body: JSON.stringify(payload)
  })
    .then(response => {
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      return response.json();
    })
    .then(data => {
      console.log('Success:', data);
    })
    .catch(error => {
      console.error('Error:', error);
    });
}