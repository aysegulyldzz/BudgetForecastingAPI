using System.ComponentModel.DataAnnotations;
using BudgetForecastingAPI.Enums;

namespace BudgetForecastingAPI.DTOs
{
    public class BudgetPredictionRequestDTO
    {
        [Required(ErrorMessage = "Departman secimi zorunludur.")]
        [Range(1,8,ErrorMessage ="Departman ID 1 ile 8 arasinda bir deger olmalidir.")]
        public int DepartmentId { get; set; }
        [Required(ErrorMessage = "Tahmin yili zorunludur.")]
        [Range(2025, 2030, ErrorMessage = "Tahmin yili 2025 ile 2030 yillari arasinda olmalidir.")]
        public int Year { get; set; }
        [Required(ErrorMessage = "Beklenen enflasyon verisi gereklidir.")]
        [Range(0.0,100.0, ErrorMessage = "Beklenen enflasyon orani %0 ile %100 arasinda olmalidir")]
        public double? ExpectedInflation { get; set; }
        [Required(ErrorMessage = "Beklenen dolar kuru verisi gereklidir.")]
        [Range(0.0,100.0, ErrorMessage = "Beklenen dolar kuru artis orani %0 ile %100 arasinda olmalidir.")]
        public double? ExpectedUsdIncrease { get; set; }
        [Required(ErrorMessage = "Beklenen altin artis verisi gereklidir.")]
        [Range(0.0,100.0, ErrorMessage = "Beklenen altin fiyati artis orani %0 ile %100 arasinda olmalidir.")]
        public double? ExpectedGoldIncrease { get; set; }
        [EnumDataType(typeof(DataSourceType), ErrorMessage = "Gecersiz veri kaynagi secildi")]
        public DataSourceType DataSource { get; set; } = DataSourceType.Database;
    }
}
