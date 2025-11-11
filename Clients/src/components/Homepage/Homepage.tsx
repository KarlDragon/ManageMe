import { Logout } from "../Logout"
import { useAuthContext } from "../../hooks/AuthContext";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import './Homepage.css'
export { Homepage }

function Homepage() {
  const navigate = useNavigate();
  const [hierarchyState, setHierarchyState] = useState<"daily" | "monthly" | "yearly">("daily");
  const { setUserEmail } = useAuthContext();
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
              <div>Giao diện chi tiêu hằng ngày</div>
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
