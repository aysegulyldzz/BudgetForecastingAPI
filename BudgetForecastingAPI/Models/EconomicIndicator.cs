using System.ComponentModel.DataAnnotations;

namespace BudgetForecastingAPI.Models
{
    public class EconomicIndicator
    {
        [Key]
        public int Id { get; set; }

        public int Year { get; set; }

        // Yıllık enflasyon oranı (Örn: 45.5)
        public decimal InflationRate { get; set; }

        // Yıl ortalaması veya yıl sonu Dolar/TL kuru
        public decimal UsdExchangeRate { get; set; }

        // Gram altın fiyatı (TL cinsinden)
        public decimal GoldPriceGram { get; set; }
    }
}