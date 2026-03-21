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
      <header>
        <h3>{hierarchyState === "daily" ? "Chi tiêu hằng ngày" : hierarchyState === "monthly" ? "Chi tiêu hằng tháng" : "Chi tiêu hằng năm"}</h3>
        <div>Tổng chi tiêu: {data.total}</div>
      </header>

      <ul>
        {data.byCategory.map((item) => (
          <li key={item.category}>
            {item.category}: {item.amount}
          </li>
        ))}
      </ul>
    </section>
  );
}