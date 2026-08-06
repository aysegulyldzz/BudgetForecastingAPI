using BudgetForecastingAPI.DTOs;
using BudgetForecastingAPI.Enums;
using BudgetForecastingAPI.Exceptions;
using BudgetForecastingAPI.Services;
using Microsoft.AspNetCore.Mvc;


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
            try
            {
                var result = await _predictionService.PredictBudgetAsync(request);
                return Ok(result);
            }
            catch (ExternalApiException ex)
            {
                string userMessage;
                switch (ex.ErrorCode)
                {
                    case ExternalApiErrorCode.DataNotFound:
                        return NotFound(new
                        {
                            Error = "Secilen departmana ait gecmis butce verisi bulunamadi.",
                            ErrorCode = (int)ex.ErrorCode
                        });

                    case ExternalApiErrorCode.ConnectionFailed:
                        userMessage = "Harici veri kaynagina su an erisilemiyor. Lutfen daha sonra tekrar deneyiniz.";
                        break;

                    case ExternalApiErrorCode.Timeout:
                        userMessage = "Harici veri servisinden yanit alinirken zaman asimina ugrandi.";
                        break;

                    case ExternalApiErrorCode.ServiceError:
                        userMessage = "Harici veri servisi gecici olarak hizmet veremiyor.";
                        break;

                    default:
                        userMessage = "Veri saglayici servisinde bilinmeyen bir hata olustu.";
                        break;
                }

                return StatusCode(503, new { Error = userMessage, ErrorCode = (int)ex.ErrorCode });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch(NotSupportedException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Error = "Sistemde beklenmeyen bir hata olustu." });
            }
        }
    }
}