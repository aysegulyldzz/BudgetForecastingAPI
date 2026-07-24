namespace BudgetForecastingAPI.DTOs
{
    public class EconomicIndicatorDTO
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public decimal InflationRate { get; set; }
        public decimal UsdExchangeRate { get; set; }
        public decimal GoldPriceGram { get; set; }
    }
}