using System;
using System.Linq;
using System.Threading.Tasks;
using BudgetForecastingAPI.Data;
using BudgetForecastingAPI.Services;
using BudgetForecastingAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetForecastingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetPredictionService _predictionService;

        public BudgetController(IBudgetPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        [HttpPost("predict")]
        public async Task<IActionResult> PredictBudget([FromBody] BudgetPredictionRequestDTO request)
        {
            if(request == null)
            {
                return BadRequest(new { message = "Istek verisi bos olamaz." });
            }

            try
            {
                var response = await _predictionService.PredictBudgetAsync(request);
                return Ok(response);
            }

            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch(Exception)
            {
                return StatusCode(500, new { message = "Hesaplamada beklenmeyen bir hata olustu." });
            }
        }
    }
}