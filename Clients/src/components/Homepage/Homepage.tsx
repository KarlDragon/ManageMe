import { Logout } from "../Logout"
import { useAuthContext } from "../../hooks/AuthContext";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { CategorySelector } from "../CategorySelector/CategorySelector";
import { HandleUserContent } from "../../handler/HandleUserContent";
import './Homepage.css'
export { Homepage }

function Homepage() {
  const navigate = useNavigate();
  const [hierarchyState, setHierarchyState] = useState<"daily" | "monthly" | "yearly">("daily");
  const [currentCategory, setCurrentCategory] = useState<
    "Breakfast" | "Lunch" | "Dinner" | "Transport" | "Entertainment" | "Shopping" >("Breakfast");
  const [moneyAmount, setMoneyAmount] = useState<number>(0);
  const [note, setNote] = useState<string>("");
  const { setUserEmail } = useAuthContext();
  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    HandleUserContent(currentCategory, moneyAmount, note);
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
                  <input type="number" name="amount" onChange={(e) => setMoneyAmount(Number(e.target.value))} required />
                </label>
                <label>
                  Ghi chú:
                  <input type="text" name="note" onChange={(e) => setNote(e.target.value)} />
                </label>
                <button type="submit" >Thêm Chi Tiêu</button>
              </form>
            ) : hierarchyState === "monthly" ? (
              <div>Giao diện chi tiêu hằng tháng</div>
            ) : (
              <div>Giao diện chi tiêu hằng năm</div>
            )}
          </div>
        </div>

      </main>
    </div>
  )
}
