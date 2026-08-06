using BudgetForecastingAPI.Enums;
using BudgetForecastingAPI.Models;

namespace BudgetForecastingAPI.Services.Providers
{
    public interface IBudgetDataProvider
    {
        DataSourceType SourceType { get; }
        Task<List<HistoricalBudgetData>> GetHistoricalBudgetsAsync(string departmentName);
    }
}
