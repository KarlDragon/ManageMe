export interface UserContent{
    category: "Breakfast" | "Lunch" | "Dinner" | "Transport" | "Entertainment" | "Shopping";
    amount: number;
    note: string;
}