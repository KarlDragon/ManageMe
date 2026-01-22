import { useState, useEffect, useCallback } from "react";
import type { SpendingData } from "../models/SpedingData";

export function useSpendingData(hierarchy: "daily" | "monthly" | "yearly") {
  const [data, setData] = useState<SpendingData | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchData = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await fetch(
        `http://localhost:5169/api/UserContent/spending/${hierarchy}`,
        { credentials: "include" }
      );
      if (!response.ok) throw new Error("Failed to fetch spending data");
      const result = await response.json();
      setData(result);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Unknown error");
    } finally {
      setLoading(false);
    }
  }, [hierarchy]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  return { data, loading, error, refetch: fetchData };
}