using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BudgetForecastingAPI.Enums;
using BudgetForecastingAPI.Models;



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
            var response = await _httpClient.GetFromJsonAsync<List<HistoricalBudgetData>>($"https://api.haricisirket.xyz.gg/budgets?department={departmentName}");

            if (response == null || response.Count == 0)
            {
                throw new KeyNotFoundException($"Dis API servisinden ({departmentName}) icin gecmis veriler alinamadi");
            }

            return response;
        }
    }
}
