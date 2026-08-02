using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetForecastingAPI.Data;
using BudgetForecastingAPI.DTOs;
using BudgetForecastingAPI.Services;

namespace BudgetForecastingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetPredictionService _predictionService;
        private readonly AppDbContext _context;

        public BudgetController(IBudgetPredictionService predictionService, AppDbContext context)
        {
            _predictionService = predictionService;
            _context = context;
        }

        [HttpPost("predict")]
        public async Task<IActionResult> PredictBudget([FromBody] BudgetPredictionRequestDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.DepartmentName))
            {
                return BadRequest(new { message = "Departman adı boş olamaz." });
            }

            // 1. Veritabanından seçilen departmana ait tüm geçmiş bütçeleri çekiyoruz
            var gecmisButcelerDb = await _context.DepartmentBudgets
                .Where(b => b.DepartmentName.ToLower() == request.DepartmentName.ToLower())
                .OrderBy(b => b.Year)
                .Select(b => new DepartmentBudgetDTO
                {
                    Id = b.Id,
                    DepartmentName = b.DepartmentName,
                    Year = b.Year,
                    AllocatedBudget = b.AllocatedBudget,
                    ActualSpent = b.ActualSpent
                })
                .ToListAsync();

            if (gecmisButcelerDb.Count < 2)
            {
                return BadRequest(new { message = $"Veritabanında '{request.DepartmentName}' departmanına ait en az 2 yıllık geçmiş bütçe verisi bulunamadı." });
            }

            // 2. Veritabanından en son yıla ait geçmiş ekonomik göstergeyi çekiyoruz
            var enSonEkonomikGostergeDb = await _context.EconomicIndicators
                .OrderByDescending(e => e.Year)
                .FirstOrDefaultAsync();

            var gecmisEkonomikDTO = enSonEkonomikGostergeDb != null ? new EconomicIndicatorDTO
            {
                Id = enSonEkonomikGostergeDb.Id,
                Year = enSonEkonomikGostergeDb.Year,
                InflationRate = enSonEkonomikGostergeDb.InflationRate,
                UsdExchangeRate = enSonEkonomikGostergeDb.UsdExchangeRate,
                GoldPriceGram = enSonEkonomikGostergeDb.GoldPriceGram
            } : new EconomicIndicatorDTO();

            // 3. Veritabanından çekilen verileri request nesnesine dolduruyoruz
            request.GecmisButceler = gecmisButcelerDb;
            request.GecmisEkonomikGosterge = gecmisEkonomikDTO;

            try
            {
                var response = _predictionService.PredictBudget(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}