import type { SpendingData } from "../../models/SpedingData";

export function UserExpense({
  hierarchyState,
  data,
  loading,
  error,
}: {
  hierarchyState: "daily" | "monthly" | "yearly";
  data: SpendingData | null;
  loading: boolean;
  error?: string | null;
}) {
  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  if (!data) {
    return <div>No spending data available.</div>;
  }

  return (
    <section className="userExpense">
      {hierarchyState === "daily" && (
        <div className="dailyExpense">
          <h3>Chi tiêu hằng ngày</h3>
          <p>Tổng chi tiêu: {data.total}</p>
        </div>
      ) || hierarchyState === "monthly" ? (
        <div className="monthlyExpense">
          <h3>Chi tiêu hằng tháng</h3>
          <p>Tổng chi tiêu: {data.total}</p>
        </div>
      ) : (
        <div className="yearlyExpense">
          <h3>Chi tiêu hằng năm</h3>
          <p>Tổng chi tiêu: {data.total}</p>
        </div>
      )
      }
    </section>
  );
}