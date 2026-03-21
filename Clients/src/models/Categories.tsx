export type Category =
  | "Breakfast"
  | "Lunch"
  | "Dinner"
  | "Transport"
  | "Entertainment"
  | "Shopping";

export interface CategorySelectorProps {
  currentCategory: Category;
  setCurrentCategory: (category: Category) => void;
}
