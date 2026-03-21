import { useState } from "react";
import "./LoginRegisterForm.css";
import { Eye, EyeOff } from "lucide-react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../hooks/AuthContext.tsx";

export { LoginForm };

function LoginForm() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [rememberMe, setRememberMe] = useState(false);
  const [formError, setFormError] = useState<string | null>(null);

  const navigate = useNavigate();
  const { login, isLoading } = useAuth();

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setFormError(null);

    try {
      await login({ email, password, rememberMe });
      navigate("/homepage");
    } catch (err) {
      setFormError(err instanceof Error ? err.message : "Login failed");
    }
  };

  const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setRememberMe(e.target.checked);
  };

  return (
    <div className="loginForm">
      <form onSubmit={handleSubmit}>
        <h2 className="formTitle">Login</h2>

        {formError ? <div className="formError">{formError}</div> : null}

        {/* Email Field */}
        <div className="formGroup">
          <label htmlFor="email">Email:</label>
          <input
            type="email"
            id="email"
            name="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>

        {/* Password Field */}
        <div className="formGroup">
          <label htmlFor="password">Password:</label>
          <input
            type={showPassword ? "text" : "password"}
            id="password"
            name="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
          <button type="button" onClick={() => setShowPassword(!showPassword)}>
            {showPassword ? <EyeOff size={30} /> : <Eye size={30} />}
          </button>
        </div>

        <label className="checkbox-btn">
          <input
            type="checkbox"
            name="rememberme"
            id="rememberme"
            checked={rememberMe}
            onChange={handleCheckboxChange}
          />
          <span>Remember me</span>
        </label>

        {/* Submit */}
        <button type="submit" className="submitButton" disabled={isLoading}>
          {isLoading ? "Logging in…" : "Login"}
        </button>
      </form>
    </div>
  );
}
