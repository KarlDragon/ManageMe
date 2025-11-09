import { useState, useEffect } from "react";

export const useAuthContext = () => {
    const [userEmail, setUserEmail] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        fetch('http://localhost:5169/api/auth/status', {
            method: 'GET',
            credentials: 'include'
        }).then(response => {
            if (response.ok) {
                return response.json();
            } else {
                throw new Error('Not authenticated');
            }
        }).then(data => { setUserEmail(data.user); console.log("Authenticated user:", data.user); })
        .catch(() => { setUserEmail(null); })
        .finally(() => setLoading(false));
        
    }, []);
    return {userEmail,setUserEmail, loading};
}