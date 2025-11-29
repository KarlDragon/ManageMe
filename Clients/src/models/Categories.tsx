export interface categories {
    currentCategory: "Breakfast" | "Lunch" | "Dinner" | "Transport" | "Entertainment"| "Shopping";
    setCurrentCategory: (category : any) => void;
}