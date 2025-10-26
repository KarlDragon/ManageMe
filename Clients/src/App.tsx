import { useState } from 'react'
import './App.css'
import { LoginForm } from './components/LoginRegisterForm/LoginForm.tsx'
import { RegisterForm } from './components/LoginRegisterForm/RegisterForm.tsx'
export { App }

function App() {
  const [AppState, setAppState] = useState('login');

  return (
    <div className="app">
      <header>
        <nav className="navbar">
          <h2 className="navbar-logo">Welcome to ManageMe</h2>
        </nav>
      </header>
      <main>
        <div className="formContainer">
          <div className="formCard">
            {AppState === 'login' && <LoginForm />}
            {AppState === 'register' && <RegisterForm />}

            <div className="toggleLinks">
              {AppState === 'login' ? (
                <p>
                  Don’t have an account?{' '}
                  <button className="linkButton" onClick={() => setAppState('register')}>
                    Register here
                  </button>
                </p>
              ) : (
                <p>
                  Already have an account?{' '}
                  <button className="linkButton" onClick={() => setAppState('login')}>
                    Login here
                  </button>
                </p>
              )}
            </div>
          </div>
        </div>
      </main>

    </div>
  )
}