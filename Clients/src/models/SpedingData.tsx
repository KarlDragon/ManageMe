export interface SpendingData {
  total: number;
  byCategory: { category: string; amount: number }[];
}