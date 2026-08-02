using System.Collections.Generic;

namespace BudgetForecastingAPI.DTOs
{
    public class BudgetPredictionRequestDTO
    {
        public string DepartmentName { get; set; } = string.Empty;
        public int TargetYear { get; set; }
        public List<DepartmentBudgetDTO> GecmisButceler { get; set; } = new();
        public EconomicIndicatorDTO GecmisEkonomikGosterge { get; set; } = new();
        public EconomicIndicatorDTO BeklenenEkonomikGosterge { get; set; } = new();
    }
}