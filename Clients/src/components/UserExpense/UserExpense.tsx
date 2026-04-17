import type { SpendingData } from "../../models/SpedingData";
import { ModifyContent } from "../ModifyContent/ModifyContent";
import type { Category } from "../../models/Categories";
import "./UserExpense.css";
import { useState } from "react";

export function UserExpense({
  hierarchyState,
  data,
  loading,
  error,
}: {
  hierarchyState: "daily" | "monthly" | "yearly";
  data: SpendingData | null;
  loading: boolean;
  error?: string | null;
}) {
  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  if (!data) {
    return <div>No spending data available.</div>;
  }
  const now = new Date();
  const offsetInMinutes = now.getTimezoneOffset();

  const [editingItem, setEditingItem] = useState<{ id: number; category: Category; moneySpent: number; note: string; date: string } | null>(null);
  return (
    <section className="userExpense">
      {hierarchyState === "daily" ? (
        <div className="dailyExpense">
          <div className="totalExpense">
            <h3>Chi tiêu hằng ngày</h3>
            <p>Tổng chi tiêu: {data.total}</p>
          </div>

          <div className="expenseItems">
            <h4>Chi tiết chi tiêu:</h4>
            {data.byItems.map((item, index) => (
              <div key={index} className="expenseItem">
                <div className="itemInfo">
                  <p><strong>Danh mục:</strong> {item.category}</p>
                  <p className="money"><strong>Số tiền:</strong> {item.moneySpent}</p>
                  <p><strong>Ghi chú:</strong> {item.note}</p>
                  <p className="time">Lúc: {new Date(new Date(item.date).getTime() - (offsetInMinutes * 60000)).toLocaleTimeString()}</p>
                </div>
                <div className="expenseActions">
                  <button className="modifyContentbtn" onClick={() => setEditingItem({ id: item.id, category: item.category as Category, moneySpent: item.moneySpent, note: item.note, date: item.date })}>
                    Sửa
                  </button>
                  <button className="removeContentbtn">Xóa</button>
                </div>
              </div>
            ))}
          </div>
        </div>
      ) : hierarchyState === "monthly" ? (
        <div className="monthlyExpense">
          <h3>Chi tiêu hằng tháng</h3>
          <p>Tổng chi tiêu: {data.total}</p>
        </div>
      ) : (
        <div className="yearlyExpense">
          <h3>Chi tiêu hằng năm</h3>
          <p>Tổng chi tiêu: {data.total}</p>
        </div>
      )}

      {editingItem && (
        <ModifyContent 
          id={editingItem.id}
          category={editingItem.category} 
          moneySpent={editingItem.moneySpent} 
          note={editingItem.note} 
          onClose={() => setEditingItem(null)} 
        />
      )}
    </section>
  );
}