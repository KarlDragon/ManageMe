import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import './App.css';
import { LoginForm } from './components/LoginRegisterForm/LoginForm.tsx';
import { RegisterForm } from './components/LoginRegisterForm/RegisterForm.tsx';
import { Homepage } from './components/Homepage/Homepage.tsx';
import { useAuthContext } from './hooks/AuthContext.tsx';
import { ToggleLinks } from './components/Togglelinks/ToggleLinks.tsx';
import { FormLayout } from './components/Layouts/FormLayout/FormLayout.tsx';
import { HomepageLayout } from './components/Layouts/HomepageLayout/HomepageLayout.tsx';

export default function App() {
  const { userEmail, loading } = useAuthContext();
  if (loading) return <div>Loading...</div>;

  const isAuthenticated = userEmail !== undefined;
  console.log("App isAuthenticated:", isAuthenticated);

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
            <HomepageLayout>
              <Homepage />
            </HomepageLayout>
          }
        />

        <Route
          path="/login"
          element={
            <FormLayout>
              <LoginForm />
              <ToggleLinks />
            </FormLayout>
          }
        />
        <Route
          path="/register"
          element={
            <FormLayout>
              <RegisterForm />
              <ToggleLinks />
            </FormLayout>
          }
        />
      </Routes>
    </Router>
  );
}
