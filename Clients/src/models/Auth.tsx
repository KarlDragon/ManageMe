export interface LoginData {
    email: string;
    password: string;
    rememberMe?: boolean;
}

export interface RegisterData{
    email: string;
    password: string;
    confirmPassword: string;
}