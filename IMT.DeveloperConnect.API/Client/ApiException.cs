using NLog;
using System;
using System.Net;

namespace IMT.DeveloperConnect.API.Client.Http
{
    public class ApiException : Exception
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IApiRequest _request;

        public ApiException(IApiRequest request, string message, Exception innerException, RequestStatus requestStatus) : base(message, innerException)
        {
            _request = request;
            RequestStatus = requestStatus;
            LogError();
        }

        public ApiException(IApiRequest request, string message, HttpStatusCode statusCode, RequestStatus requestStatus) : base(message)
        {
            _request = request;
            StatusCode = statusCode;
            RequestStatus = requestStatus;
            LogError();
        }

        public ApiException(IApiRequest request, string message, HttpStatusCode statusCode, ApiError error, RequestStatus requestStatus) : base(message)
        {
            _request = request;
            StatusCode = statusCode;
            RequestStatus = requestStatus;
            Error = error;
            LogError();
        }

        public ApiException(IApiRequest request, string message, HttpStatusCode statusCode, Exception innerException, RequestStatus requestStatus) : base(message, innerException)
        {
            _request = request;
            StatusCode = statusCode;
            RequestStatus = requestStatus;
            LogError();
        }

        public HttpStatusCode StatusCode { get; }

        public ApiError Error { get; }

        public RequestStatus RequestStatus { get; }

        private static string GetFullStackTrace(Exception ex)
        {
            if (ex == null) return null;

            return $"{GetFullStackTrace(ex.InnerException)}[{ex.GetType().FullName}] {ex.Message}" +
                $"{Environment.NewLine}{ex.StackTrace ?? string.Empty}" +
                $"{Environment.NewLine}";
        }

        private void LogError()
        {
            var logMessage = $"!!! Error in request [{_request.Method}] {_request.RequestUrl} Request Status: {RequestStatus}" +
                $"{Environment.NewLine}{Message}";
            if (InnerException != null)
                logMessage += $"{Environment.NewLine}{GetFullStackTrace(InnerException)}";
            if (Error != null)
                logMessage += $"{Environment.NewLine}{Newtonsoft.Json.JsonConvert.SerializeObject(Error, Newtonsoft.Json.Formatting.Indented)}";

            _logger.Error(logMessage);

        }
    }


}
