using BudgetForecastingAPI.DTOs;
using BudgetForecastingAPI.Services.Providers;

namespace BudgetForecastingAPI.Services
{
    public class BudgetPredictionManager : IBudgetPredictionService
    {
        private readonly IEnumerable<IBudgetDataProvider> _dataProviders;

        public BudgetPredictionManager(IEnumerable<IBudgetDataProvider> dataProviders)
        {
            _dataProviders = dataProviders;
        }

        public async Task<BudgetPredictionResponseDTO> PredictBudgetAsync(BudgetPredictionRequestDTO request)
        {
            string[] deptNames = { "", "Operasyon / Üretim", "Satış & Pazarlama", "İnsan Kaynakları", "Finans & Muhasebe", "Bilgi Teknolojileri (IT)", "Ar-Ge", "Genel Yönetim (G&A)", "Lojistik / Tedarik Zinciri" };

            if (request.DepartmentId < 1 || request.DepartmentId >= deptNames.Length)
            {
                throw new ArgumentException("Gecersiz departman secimi.");
            }

            string targetName = deptNames[request.DepartmentId];

            var provider = _dataProviders.FirstOrDefault(p => p.SourceType == request.DataSource) ?? throw new NotSupportedException($"Secilen veri kaynagi ({request.DataSource}) desteklenmiyor.");

            var historicalBudgets = await provider.GetHistoricalBudgetsAsync(targetName);
            var lastBudget = historicalBudgets.OrderByDescending(b => b.Year).FirstOrDefault();

            if (lastBudget == null)
            {
                throw new KeyNotFoundException($"Ilgili kaynaktan {targetName} icin gecerli veri alinamadi.");
            }

            double w1 = request.ExpectedInflation.Value / 100.0;
            double w2 = request.ExpectedUsdIncrease.Value / 100.0;
            double w3 = request.ExpectedGoldIncrease.Value / 100.0;

            double multiplier = 1.0;

            switch (request.DepartmentId)
            {
                case 1: // Operasyon / Üretim
                    multiplier = 1 + (1.6768 * w1) - (0.1251 * w2) + (0.1026 * w3);
                    break;
                case 2: // Satış & Pazarlama
                    multiplier = 1 + (1.7325 * w1) - (0.0553 * w2) + (0.0948 * w3);
                    break;
                case 3: // İnsan Kaynakları
                    multiplier = 1 + (1.7190 * w1) - (0.0866 * w2) + (0.0982 * w3);
                    break;
                case 4: // Finans & Muhasebe
                    multiplier = 1 + (1.7090 * w1) - (0.1002 * w2) + (0.0997 * w3);
                    break;
                case 5: // Bilgi Teknolojileri (IT)
                    multiplier = 1 + (1.3578 * w1) + (0.8821 * w2) - (0.0045 * w3);
                    break;
                case 6: // Ar-Ge
                    multiplier = 1 + (1.7006 * w1) + (0.1962 * w2) + (0.0676 * w3);
                    break;
                case 7: // Genel Yönetim (G&A)
                    multiplier = 1 + (1.7394 * w1) - (0.0124 * w2) + (0.0901 * w3);
                    break;
                case 8: // Lojistik / Tedarik Zinciri
                    multiplier = 1 + (1.7360 * w1) + (0.0545 * w2) + (0.0828 * w3);
                    break;
                default:
                    throw new ArgumentException("Gecersiz departman secimi.");
            }

            decimal oncekiHarcama = lastBudget.ActualSpent;
            decimal tahminiButce = oncekiHarcama * (decimal)multiplier;
            double yuzdeDegisim = (multiplier - 1.0) * 100.0;

            return new BudgetPredictionResponseDTO
            {
                DepartmentName = targetName,
                TahminYili = request.Year,
                OncekiYilHarcamasi = oncekiHarcama,
                TahminiButce = Math.Round(tahminiButce, 2),
                YuzdeDegisim = Math.Round(yuzdeDegisim, 2),
                Aciklama = $"Analiz modeli üzerinden Enflasyon: %{request.ExpectedInflation}, Dolar: %{request.ExpectedUsdIncrease}, Altın: %{request.ExpectedGoldIncrease} parametreleriyle hesaplanmıştır."
            };
        }
    }
}

