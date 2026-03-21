import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import "./App.css";
import { LoginForm } from "./components/LoginRegisterForm/LoginForm.tsx";
import { RegisterForm } from "./components/LoginRegisterForm/RegisterForm.tsx";
import { Homepage } from "./components/Homepage/Homepage.tsx";
import { useAuth } from "./hooks/AuthContext.tsx";
import { ToggleLinks } from "./components/ToggleLinks.tsx";
import { FormLayout } from "./components/Layouts/FormLayout/FormLayout.tsx";
import { HomepageLayout } from "./components/Layouts/HomepageLayout/HomepageLayout.tsx";

export default function App() {
  const { isAuthenticated, isLoading } = useAuth();

  if (isLoading) {
    return <div>Loading...</div>;
  }

  return (
    <Router>
      <Routes>
        <Route
          path="/"
          element={<Navigate to={isAuthenticated ? "/homepage" : "/login"} replace />}
        />

        <Route
          path="/homepage"
          element={
            isAuthenticated ? (
              <HomepageLayout>
                <Homepage />
              </HomepageLayout>
            ) : (
              <Navigate to="/login" replace />
            )
          }
        />

        <Route
          path="/login"
          element={
            isAuthenticated ? (
              <Navigate to="/homepage" replace />
            ) : (
              <FormLayout>
                <LoginForm />
                <ToggleLinks />
              </FormLayout>
            )
          }
        />
        <Route
          path="/register"
          element={
            isAuthenticated ? (
              <Navigate to="/homepage" replace />
            ) : (
              <FormLayout>
                <RegisterForm />
                <ToggleLinks />
              </FormLayout>
            )
          }
        />
      </Routes>
    </Router>
  );
}
