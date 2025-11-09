import { useState } from 'react';
import { Eye, EyeOff } from "lucide-react";
import { handleRegister } from '../../handler/HandleLoginRegister.tsx';
import { useNavigate } from "react-router-dom";
// import './LoginRegisterForm.css';
export { RegisterForm };

function RegisterForm() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const navigate = useNavigate();
  function handleSubmit(event: React.FormEvent) {
    event.preventDefault();
    if (password !== confirmPassword) {
      alert("Passwords do not match!");
      return;
    }
    handleRegister({ email, password, confirmPassword }, navigate);
  }

  return (
    <div className="loginForm">
      <form action="/register" method="POST" onSubmit={handleSubmit}>
        <h2 className="formTitle">Register</h2>

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
          <button
            type="button"
            onClick={() => setShowPassword(!showPassword)}
          >
            {showPassword ? <EyeOff size={30} /> : <Eye size={30} />}
          </button>
        </div>

        {/* Confirm Password Field */}
        <div className="formGroup">
          <label htmlFor="confirmPassword">Confirm Password:</label>
          <input
            type={showConfirmPassword ? "text" : "password"}
            id="confirmPassword"
            name="confirmPassword"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            required
          />
          <button
            type="button"
            onClick={() => setShowConfirmPassword(!showConfirmPassword)}
          >
            {showConfirmPassword ? <EyeOff size={30} /> : <Eye size={30} />}
          </button>
        </div>

        {/* Submit */}
        <button type="submit" className="submitButton">
          Register
        </button>
      </form>
    </div>
  );
}
