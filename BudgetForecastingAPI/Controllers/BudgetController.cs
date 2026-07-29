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

            if(request == null){
                return BadRequest(new {message = "Istek verisi bos olamaz."});
            }

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            try{
                var response = _predictionService.PredictBudget(request);
                return Ok(response);
            }
            catch(Exception ex){
                return StatusCode(500, new { message = "Hesaplamada bir hata olustu.", detail = ex.Message });
            }
            
        }
    }
}
