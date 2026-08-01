using BudgetForecastingAPI.DTOs;

namespace BudgetForecastingAPI.Services
{
    public interface IBudgetPredictionService
    {
        BudgetPredictionResponseDTO PredictBudget(BudgetPredictionRequestDTO request);
    }
}
