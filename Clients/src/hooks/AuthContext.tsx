import React, { createContext, useContext, useEffect, useMemo, useState } from "react";
import type { LoginData, RegisterData } from "../models/Auth";
import type { AuthStatus } from "../models/AuthStatus";
import * as authService from "../services/authService";

type AuthContextValue = {
  userEmail: string | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  error: string | null;
  login: (data: LoginData) => Promise<void>;
  register: (data: RegisterData) => Promise<void>;
  logout: () => Promise<void>;
};

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [userEmail, setUserEmail] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function checkAuth() {
      setIsLoading(true);
      setError(null);
      try {
        const data: AuthStatus = await authService.getAuthStatus();
        setUserEmail(data.user);
      } catch {
        setUserEmail(null);
      } finally {
        setIsLoading(false);
      }
    }

    checkAuth();
  }, []);

  const login = async (credentials: LoginData) => {
    setIsLoading(true);
    setError(null);
    try {
      const result = await authService.login(credentials);
      setUserEmail(result.user);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Login failed");
      throw err;
    } finally {
      setIsLoading(false);
    }
  };

  const register = async (data: RegisterData) => {
    setIsLoading(true);
    setError(null);
    try {
      const result = await authService.register(data);
      setUserEmail(result.user);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Registration failed");
      throw err;
    } finally {
      setIsLoading(false);
    }
  };

  const logout = async () => {
    setIsLoading(true);
    setError(null);
    try {
      await authService.logout();
      setUserEmail(null);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Logout failed");
      throw err;
    } finally {
      setIsLoading(false);
    }
  };

  const value = useMemo(
    () => ({
      userEmail,
      isAuthenticated: Boolean(userEmail),
      isLoading,
      error,
      login,
      register,
      logout,
    }),
    [userEmail, isLoading, error]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
}
