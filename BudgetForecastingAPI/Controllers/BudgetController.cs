using BudgetForecastingAPI.DTOs;
using BudgetForecastingAPI.Enums;
using BudgetForecastingAPI.Exceptions;
using BudgetForecastingAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;


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

        
        [HttpGet("export/scenario-excel")]
        public IActionResult ExportScenarioExcel(
            [FromQuery] string department,
            [FromQuery] int year,
            [FromQuery] decimal inflation,
            [FromQuery] decimal dollar,
            [FromQuery] decimal gold,
            [FromQuery] decimal previousBudget,
            [FromQuery] decimal predictedBudget,
            [FromQuery] decimal changePercentage)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Parametre / Metrik;Değer");
            sb.AppendLine($"Departman;{department}");
            sb.AppendLine($"Tahmin Yılı;{year}");
            sb.AppendLine($"Beklenen Enflasyon;%{inflation:F2}");
            sb.AppendLine($"Beklenen Dolar Artışı;%{dollar:F2}");
            sb.AppendLine($"Beklenen Altın Artışı;%{gold:F2}");
            sb.AppendLine("----------------------------------;----------------------------------");
            sb.AppendLine($"Önceki Yıl Harcaması;{previousBudget:N0} ₺");
            sb.AppendLine($"Değişim Oranı;%{changePercentage:F2}");
            sb.AppendLine($"TAHMİNİ BÜTÇE İHTİYACI;{predictedBudget:N0} ₺");

            byte[] buffer = Encoding.UTF8.GetBytes(sb.ToString());
            byte[] bom = Encoding.UTF8.GetPreamble();
            return File(bom.Concat(buffer).ToArray(), "text/csv", $"Butce_Raporu_{department}_{year}.csv");
        }

        
        [HttpGet("export/scenario-pdf")]
        public IActionResult ExportScenarioPdf(
            [FromQuery] string department,
            [FromQuery] int year,
            [FromQuery] decimal inflation,
            [FromQuery] decimal dollar,
            [FromQuery] decimal gold,
            [FromQuery] decimal previousBudget,
            [FromQuery] decimal predictedBudget,
            [FromQuery] decimal changePercentage)
        {
            string html = $@"
    <!DOCTYPE html>
    <html lang='tr'>
    <head>
        <meta charset='utf-8' />
        <title>Uyumsoft | Bütçe Simülasyon Raporu</title>
        <style>
            body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; padding: 40px; background: #f8f9fa; color: #212529; }}
            .card {{ background: #fff; border: 1px solid #dee2e6; border-radius: 12px; padding: 28px; max-width: 580px; margin: 0 auto; box-shadow: 0 4px 12px rgba(0,0,0,0.05); }}
            h2 {{ color: #E30613; border-bottom: 2px solid #E30613; padding-bottom: 10px; margin-top: 0; font-size: 20px; }}
            .row {{ display: flex; justify-content: space-between; padding: 10px 0; border-bottom: 1px solid #f1f3f5; font-size: 14px; }}
            .label {{ color: #6c757d; font-weight: 500; }}
            .value {{ font-weight: 600; }}
            .total-box {{ background: rgba(227,6,19,0.05); border: 1px solid rgba(227,6,19,0.2); border-radius: 8px; padding: 16px; margin-top: 20px; text-align: center; }}
            .total-title {{ color: #E30613; font-size: 12px; font-weight: bold; text-transform: uppercase; }}
            .total-amount {{ color: #E30613; font-size: 26px; font-weight: 800; margin-top: 4px; }}
            @media print {{
                body {{ background: #fff; padding: 0; }}
                .card {{ border: none; box-shadow: none; width: 100%; max-width: 100%; }}
                .no-print {{ display: none; }}
            }}
        </style>
    </head>
    <body onload='window.print()'>
        <div class='card'>
            <h2>📊 Uyumsoft Bütçe Simülasyon Raporu</h2>
            <div class='row'><span class='label'>Departman:</span><span class='value'>{WebUtility.HtmlEncode(department)}</span></div>
            <div class='row'><span class='label'>Tahmin Yılı:</span><span class='value'>{year}</span></div>
            <div class='row'><span class='label'>Beklenen Enflasyon:</span><span class='value'>%{inflation:F1}</span></div>
            <div class='row'><span class='label'>Beklenen Dolar Artışı:</span><span class='value'>%{dollar:F1}</span></div>
            <div class='row'><span class='label'>Beklenen Altın Artışı:</span><span class='value'>%{gold:F1}</span></div>
            <div class='row'><span class='label'>Önceki Yıl Harcaması:</span><span class='value'>{previousBudget:N0} ₺</span></div>
            <div class='row'><span class='label'>Değişim Oranı:</span><span class='value'>%{changePercentage:F1}</span></div>
            
            <div class='total-box'>
                <div class='total-title'>Tahmini Bütçe İhtiyacı</div>
                <div class='total-amount'>{predictedBudget:N0} ₺</div>
            </div>
        </div>
    </body>
    </html>";

            return Content(html, "text/html", Encoding.UTF8);
        }
    }
}