import type { LoginData, RegisterData } from "../models/Auth";
export {handleLogin, handleRegister};

function handleRegister( {email, password, confirmPassword} : RegisterData) : void {
    // Logic for handling registration will go here
    console.log("Registering new user" , email, password, confirmPassword);
}

function handleLogin({email, password} : LoginData ) : void {
    // Logic for handling login will go here
    console.log("Logging in with", email, password);
}