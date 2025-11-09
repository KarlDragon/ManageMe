export async function Logout({ setUserEmail }: { setUserEmail: (email: string | null) => void }, navigate: (path: string) => void) {
  try {
    const response = await fetch("http://localhost:5169/api/auth/logout", {
      method: "POST",
      credentials: "include",
    });

    if (response.ok) {
      console.log("Logout successful");
      setUserEmail(null);
      navigate("/login");
    } else {
      console.error("Logout failed");
    }
  } catch (err) {
    console.error("Logout error:", err);
  }
}
