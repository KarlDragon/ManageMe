import { useState } from 'react';
import './LoginRegisterForm.css';
import { handleLogin } from '../../handler/handleLoginRegister.tsx';
import { Eye, EyeOff } from 'lucide-react';

export { LoginForm };

function LoginForm() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);

  function handleSubmit(event: React.FormEvent) {
    event.preventDefault();
    handleLogin({ email, password });
  }

  return (
    <div className="loginForm">
      <form action="/login" method="POST" onSubmit={handleSubmit}>
        <h2 className="formTitle">Login</h2>

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
            type={showPassword ? 'text' : 'password'}
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

        {/* Submit */}
        <button type="submit" className="submitButton">
          Login
        </button>
      </form>
    </div>
  );
}
