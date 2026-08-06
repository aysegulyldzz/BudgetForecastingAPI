using BudgetForecastingAPI.Enums;

namespace BudgetForecastingAPI.Exceptions
{
    public class ExternalApiException : Exception
    {
        public ExternalApiErrorCode ErrorCode { get; }

        public ExternalApiException(ExternalApiErrorCode errorCode, string? message = null)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
