using System.Net;
using BudgetForecastingAPI.Enums;
using BudgetForecastingAPI.Models;
using BudgetForecastingAPI.Exceptions;



namespace BudgetForecastingAPI.Services.Providers
{
    public class ExternalApiBudgetDataProvider : IBudgetDataProvider
    {
        private readonly HttpClient _httpClient;
        public DataSourceType SourceType => DataSourceType.ExternalApi;

        public ExternalApiBudgetDataProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<HistoricalBudgetData>> GetHistoricalBudgetsAsync(string departmentName)
        {
            try
            {
                string departmentNameSafe = Uri.EscapeDataString(departmentName);
                var response = await _httpClient.GetAsync($"budgets?epartment={departmentNameSafe}");

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new ExternalApiException(ExternalApiErrorCode.DataNotFound);
                    }
                    throw new ExternalApiException(ExternalApiErrorCode.ServiceError);
                }

                var data = await response.Content.ReadFromJsonAsync<List<HistoricalBudgetData>>();

                if (data == null || !data.Any())
                {
                    throw new ExternalApiException(ExternalApiErrorCode.DataNotFound);
                }

                return data;
            }
            catch(ExternalApiException)
            {
                throw;
            }
            catch(TaskCanceledException)
            {
                throw new ExternalApiException(ExternalApiErrorCode.Timeout);
            }
            catch(HttpRequestException)
            {
                throw new ExternalApiException(ExternalApiErrorCode.ConnectionFailed);
            }
        }
    }
}
