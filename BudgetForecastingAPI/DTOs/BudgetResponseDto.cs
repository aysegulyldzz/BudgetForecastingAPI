namespace BudgetForecastingAPI.DTOs
{
    public class BudgetResponseDto
    {
        public string DepartmentName { get; set; } = string.Empty;
        public decimal PredictedBudget { get; set; }
        public decimal PercentageChange { get; set; }
        public DateTime CalculationDate { get; set; } = DateTime.UtcNow;
    }
}