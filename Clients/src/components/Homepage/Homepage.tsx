import { Logout } from "../Logout"
import { useAuthContext } from "../../hooks/AuthContext";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { CategorySelector } from "../CategorySelector/CategorySelector";
import { HandleUserContent } from "../../handler/HandleUserContent";
import { UserExpense } from "../UserExpense/UserExpense";
import { useSpendingData } from "../../hooks/useSpendingData";
import './Homepage.css'
export { Homepage }

function Homepage() {
  const navigate = useNavigate();
  const [hierarchyState, setHierarchyState] = useState<"daily" | "monthly" | "yearly">("daily");
  const [currentCategory, setCurrentCategory] = useState<
    "Breakfast" | "Lunch" | "Dinner" | "Transport" | "Entertainment" | "Shopping">("Breakfast");
  const [moneyAmount, setMoneyAmount] = useState<number>(0);
  const [note, setNote] = useState<string>("");
  const { setUserEmail } = useAuthContext();

  const { data, loading, error, refetch } = useSpendingData(hierarchyState);

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      await HandleUserContent(currentCategory, moneyAmount, note);
      setMoneyAmount(0);
      setNote("");
      refetch();
    } catch (err) {
      console.error("Failed to submit expense:", err);
    }
  }

  return (
    <div className="homepageContainer">
      <header className="homepageHeader">
        <h1>ManageMe</h1>
        <button onClick={() => { Logout({ setUserEmail }, navigate) }}>Đăng xuất</button>
      </header>
      <main className="homepageMain">
        <div className="feature">
          <div className="hierarchy">
            <h2>Chi tiêu:</h2>
            <div className="expensetimeWrapper">
              <button onClick={() => setHierarchyState("daily")}>Hằng Ngày</button>
              <button onClick={() => setHierarchyState("monthly")}>Hằng Tháng</button>
              <button onClick={() => setHierarchyState("yearly")}>Hằng Năm</button>
            </div>

          </div>
          <div className="contentArea">
            {hierarchyState === "daily" ? (
              <div className="usercontent">
                <form className="dailyCard" action="#" method="POST" onSubmit={handleSubmit}>
                  <h2>Thêm mục chi tiêu mới</h2>
                  <label>
                    Chi tiêu cho:
                  </label>
                  <CategorySelector
                    currentCategory={currentCategory}
                    setCurrentCategory={setCurrentCategory}
                  />
                  <label>
                    Số tiền:
                    <input type="number" name="amount" value={moneyAmount} onChange={(e) => setMoneyAmount(Number(e.target.value))} required />
                  </label>
                  <label>
                    Ghi chú:
                    <input type="text" name="note" value={note} onChange={(e) => setNote(e.target.value)} />
                  </label>
                  <button type="submit" >Thêm Chi Tiêu</button>
                </form>
                <div className="expense">
                  <UserExpense hierarchyState={hierarchyState} data={data} loading={loading} error={error} />
                </div>
              </div>
            ) : (
              <div className="expense">
                <UserExpense hierarchyState={hierarchyState} data={data} loading={loading} error={error} />
              </div>
            )}
          </div>
        </div>

      </main>
    </div>
  )
}
