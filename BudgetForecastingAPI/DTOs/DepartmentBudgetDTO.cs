namespace BudgetForecastingAPI.DTOs
{
    public class DepartmentBudgetDTO
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal AllocatedBudget { get; set; }
        public decimal ActualSpent { get; set; }
    }
}