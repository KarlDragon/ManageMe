import { Logout } from "../Logout"
import { useAuthContext } from "../../hooks/AuthContext";
import { useNavigate } from "react-router-dom";
import './Homepage.css'
export { Homepage }

function Homepage() {
  const navigate = useNavigate();
  const { setUserEmail } = useAuthContext();
  return (
    <div className="homepageContainer">
      <header className="homepageHeader">
        <h1>ManageMe</h1>
        <button onClick={() => { Logout({ setUserEmail }, navigate) }}>Log out</button>
      </header>
      <main className="homepageMain">
        <div className="hierarchy">

        </div>
        <div className="contentArea">

        </div>
      </main> 
    </div>
  )
}
