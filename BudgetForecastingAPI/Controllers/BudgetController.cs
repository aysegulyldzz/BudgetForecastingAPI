using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BudgetForecastingAPI.DTOs;
using BudgetForecastingAPI.Services;


namespace BudgetForecastingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetPredictionService _predictionService;

        public BudgetController(IBudgetPredictionService predictionService){
            _predictionService = predictionService;
        }

        [HttpPost("predict")]
        public IActionResult PredictBudget([FromBody] BudgetPredictionRequestDTO request){

            if(request == null || request.GecmisButceler == null || request.GecmisButceler.Count < 2){
                return BadRequest(new {message = "Istek verisi bos olamaz ve en az 2 yila ait gecmis butce verisi gereklidir."});
            }

            var years = request.GecmisButceler.Select(b => b.Year).ToList();

            if(years.Distinct().Count() != years.Count || !years.SequenceEqual(years.OrderBy(y => y))){
                return BadRequest(new { message = "Gecmis butce verilerinde ayni yila ait birden fazla kayit olamaz ve eskiden yeniye sirali olmalidir." });
            }

            if(request.BeklenenEkonomikGosterge != null && request.BeklenenEkonomikGosterge.Year <= years.Last()){
                return BadRequest(new { message = $"Beklenen butce yili ({request.BeklenenEkonomikGosterge.Year}), son gecmis yildan ({years.Last()}) buyuk olmalidir." });
            }
        
            try{
                var response = _predictionService.PredictBudget(request);
                return Ok(response);
            }
            catch (ArgumentException ex){
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception){
                return StatusCode(500, new { message = "Hesaplamada beklenmeyen bir hata olustu."});
            }
            
        }
    }
}
