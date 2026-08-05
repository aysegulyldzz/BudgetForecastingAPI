using BudgetForecastingAPI.Enums;
using System.Collections.Generic;

namespace BudgetForecastingAPI.DTOs
{
    public class BudgetPredictionRequestDTO
    {
        public int DepartmentId { get; set; }
        public int Year { get; set; }
        public double ExpectedInflation { get; set; }
        public double ExpectedUsdIncrease { get; set; }
        public double ExpectedGoldIncrease { get; set; }
        public DataSourceType DataSource { get; set; } = DataSourceType.Database;
    }
}
