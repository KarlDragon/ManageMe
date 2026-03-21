import type { Category, CategorySelectorProps } from "../../models/Categories";
import "./CategorySelector.css";

export function CategorySelector({ currentCategory, setCurrentCategory }: CategorySelectorProps) {
  const categoryList: Record<Category, string> = {
    Breakfast: "Ăn sáng",
    Lunch: "Ăn trưa",
    Dinner: "Ăn tối",
    Transport: "Di chuyển",
    Entertainment: "Giải trí",
    Shopping: "Mua sắm",
  };

  return (
    <div className="categorySelector">
      {Object.entries(categoryList).map(([key, label]) => (
        <button
          key={key}
          type="button"
          className={currentCategory === key ? "activeCategory" : ""}
          onClick={() => setCurrentCategory(key as Category)}
        >
          {label}
        </button>
      ))}
    </div>
  );
}
