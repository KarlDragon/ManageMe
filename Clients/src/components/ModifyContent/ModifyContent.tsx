import { CategorySelector } from "../CategorySelector/CategorySelector";
import { useState } from "react";
import type { Category } from "../../models/Categories";
import { modifyUserContent } from "../../services/userContentService";
import "./ModifyContent.css";

export interface ModifyContentProps {
  id: number;
  category: Category;
  moneySpent: number;
  note: string;
  onClose: () => void;
}

export function ModifyContent({ id, category, moneySpent, note, onClose }: ModifyContentProps) {
    const [selectedCategory, setSelectedCategory] = useState<Category>(category);
    const [moneyAmount, setMoneyAmount] = useState(moneySpent);
    const [noteText, setNoteText] = useState(note);

    return (
        <div className="modal-overlay">
            <div className="modal-content">
                <button className="close-button" onClick={onClose}>×</button>
                <h2>Chỉnh sửa nội dung chi tiêu</h2>
                <label>Danh mục:</label>
                <CategorySelector currentCategory={selectedCategory} setCurrentCategory={setSelectedCategory} />
                <label>Số tiền:</label>
                <input type="number" value={moneyAmount} onChange={(e) => setMoneyAmount(Number(e.target.value))} />
                <label>Ghi chú:</label>
                <input type="text" value={noteText} onChange={(e) => setNoteText(e.target.value)} />
                <div className="modal-actions">
                    <button onClick={onClose}>Hủy</button>
                    <button onClick={async () => {
                        await modifyUserContent({
                            id: id,
                            category: selectedCategory,
                            MoneySpent: moneyAmount,
                            note: noteText
                        });
                        onClose();
                    }}>Lưu</button>
                </div>
            </div>
        </div>
    );
}