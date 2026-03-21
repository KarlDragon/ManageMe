import { useState, useEffect, useCallback } from "react";
import type { SpendingData } from "../models/SpedingData";
import { getSpendingData } from "../services/userContentService";

export function useSpendingData(hierarchy: "daily" | "monthly" | "yearly") {
  const [data, setData] = useState<SpendingData | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchData = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const result = await getSpendingData(hierarchy);
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