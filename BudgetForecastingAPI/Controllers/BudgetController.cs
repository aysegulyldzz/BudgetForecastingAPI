using BudgetForecastingAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetForecastingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly AppDbContext _context; // Veritabanı bağlamın

        public BudgetController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("predict")]
        public async Task<IActionResult> PredictBudget([FromBody] BudgetPredictionRequestDTO request)
        {
            // 1. Departman ID'sini Departman İsmine Çevir
            string[] deptNames = { "", "Operasyon / Üretim", "Satış & Pazarlama", "İnsan Kaynakları", "Finans & Muhasebe", "Bilgi Teknolojileri (IT)", "Ar-Ge", "Genel Yönetim (G&A)", "Lojistik / Tedarik Zinciri" };
            string targetName = deptNames[request.DepartmentId];

            // Veritabanından o departmanın EN SON (2025) gerçekleşen bütçesini çek
            var lastBudget = await _context.DepartmentBudgets
                .Where(d => d.DepartmentName == targetName)
                .OrderByDescending(d => d.Year)
                .FirstOrDefaultAsync();

            if (lastBudget == null)
                return NotFound(new { message = "Seçilen departman için geçmiş veri bulunamadı." });

            // 2. Kullanıcıdan gelen yüzde değerlerini ondalık formata (W1, W2, W3) çevir
            double w1 = request.ExpectedInflation / 100.0;
            double w2 = request.ExpectedUsdIncrease / 100.0;
            double w3 = request.ExpectedGoldIncrease / 100.0;

            // 3. Departman bazlı matematiksel model katsayılarını uygula
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
                    return BadRequest(new { message = "Geçersiz departman seçimi." });
            }

            // 4. Bütçe ve Değişim Oranı Hesaplamaları
            decimal oncekiHarcama = lastBudget.ActualSpent;
            decimal tahminiButce = oncekiHarcama * (decimal)multiplier;
            double yuzdeDegisim = (multiplier - 1.0) * 100.0;

            // 5. Arayüzün beklediği formattaki (JSON) yanıtı oluştur
            var response = new
            {
                departmentName = lastBudget.DepartmentName,
                tahminYili = request.Year,
                oncekiYilHarcamasi = oncekiHarcama,
                tahminiButce = Math.Round(tahminiButce, 2),
                yuzdeDegisim = Math.Round(yuzdeDegisim, 2),
                aciklama = $"Analiz modeli üzerinden Enflasyon: %{request.ExpectedInflation}, Dolar: %{request.ExpectedUsdIncrease}, Altın: %{request.ExpectedGoldIncrease} parametreleriyle hesaplanmıştır."
            };

            return Ok(response);
        }
    }

    // Arayüzden gelecek JSON verilerini karşılayan DTO sınıfı
    public class BudgetPredictionRequestDTO
    {
        public int DepartmentId { get; set; }
        public int Year { get; set; }
        public double ExpectedInflation { get; set; }
        public double ExpectedUsdIncrease { get; set; }
        public double ExpectedGoldIncrease { get; set; }
    }
}