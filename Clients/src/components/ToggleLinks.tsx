import { Link, useLocation } from "react-router-dom";
export function ToggleLinks() {
    const location = useLocation();
    const isLoginPage = location.pathname === "/login";

    return (
        <div className="toggleLinks">
            {isLoginPage ? <Link to="/register">Register</Link> : <Link to="/login">Login</Link>}
        </div>
    );
}
