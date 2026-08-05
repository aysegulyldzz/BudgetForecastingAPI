namespace BudgetForecastingAPI.Models
{
    public class HistoricalBudgetData
    {
        public int departmentId { get; set; }
        public int Year { get; set; }
        public decimal ActualSpent { get; set; }
    }
}
