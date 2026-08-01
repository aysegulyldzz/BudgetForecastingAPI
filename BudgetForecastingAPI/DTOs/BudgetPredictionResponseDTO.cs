namespace BudgetForecastingAPI.DTOs
{
    public class BudgetPredictionResponseDTO
    {
        public string DepartmentName { get; set; } = string.Empty;
        public int TahminYili { get; set; }
        public decimal OncekiYilHarcamasi { get; set; }
        public decimal TahminiButce { get; set; }
        public decimal YuzdeDegisim { get; set; }
        public string Aciklama { get; set; } = string.Empty;
    }
}
