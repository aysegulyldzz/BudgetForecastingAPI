namespace BudgetForecastingAPI.DTOs
{
    public class BudgetRequestDto
    {
        public string DepartmentName { get; set; } = string.Empty;
        public decimal PastExpenditure { get; set; }
        public decimal ExpectedInflationRate { get; set; }
        public decimal ExpectedUsdRateChange { get; set; }
        public decimal ExpectedGoldRateChange { get; set; }
    }
}