using BudgetForecastingAPI.DTOs;

namespace BudgetForecastingAPI.Services
{
    public interface IBudgetPredictionService
    {
        Task<BudgetPredictionResponseDTO> PredictBudgetAsync(BudgetPredictionRequestDTO request);
    }
}
