import type { categories } from "../../models/Categories";
import './CategorySelector.css'
export function CategorySelector({ currentCategory, setCurrentCategory }: categories) {
    const categoryList = {
        "Breakfast": "Ăn sáng",
        "Lunch": "Ăn trưa",
        "Dinner": "Ăn tối",
        "Transport": "Di chuyển",
        "Entertainment": "Giải trí",
        "Shopping": "Mua sắm"
    };
    return (
        <div className="categorySelector">
            {Object.entries(categoryList).map(([key, label]) => (
                <button
                    key={key}
                    type="button"
                    className={currentCategory === key ? "activeCategory" : ""}
                    onClick={() => setCurrentCategory(key as categories["currentCategory"])}
                > {label}</button>
            ))}
        </div>
    );
}