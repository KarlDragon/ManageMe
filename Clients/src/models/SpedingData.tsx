export interface SpendingData {
  total: number;
  byCategory: { category: string; amount: number }[];
  byItems: {
    id: number;
    category: string
    moneySpent: number
    note: string
    date: string
  }[];
}