import type { LoginData, RegisterData } from "../models/Auth";
export {handleLogin, handleRegister};

function handleRegister( {email, password, confirmPassword} : RegisterData, navigate: (path: string) => void) : void {
    fetch('http://localhost:5169/api/auth/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
    }).then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    }).then(data => {
        console.log('Registration successful:', data);
        navigate("/homepage");
    }).catch(error => {
        console.error('There was a problem with the registration request:', error);
    }); 
    // Logic for handling registration will go here
    console.log("Registering new user" , email, password, confirmPassword);
}

function handleLogin({email, password, rememberMe} : LoginData, navigate: (path: string) => void) : void {
    // Logic for handling login will go here
    fetch('http://localhost:5169/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify({ email, password, rememberMe })
    }).then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    }).then(data => {
        console.log('Login successful:', data);
        navigate("/homepage");
    }).catch(error => {
        console.error('There was a problem with the login request:', error);
    });
    console.log("Logging in with", email, password, rememberMe);
}