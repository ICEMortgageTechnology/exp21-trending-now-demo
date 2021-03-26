using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Client.Logging
{
    public class ApiRequestLogger
    {
        private static readonly HashSet<string> InterestingResponseHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Location",
            "X-Total-Count",
            "X-Correlation-ID",
            "Content-Type",
            "Content-Length"
        };

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private Stopwatch watch = null;

        public void LogStart(HttpRequestMessage request)
        {
            _logger.Debug(() =>
            {
                var logMessage = $">>> Request [{request.Method}] {request.RequestUri.OriginalString}";
                if (request.Headers.Any())
                {
                    logMessage += $"{Environment.NewLine}>>> Headers:";
                    foreach (var header in request.Headers)
                    {
                        var key = header.Key;
                        var values = header.Value;
                        if (string.Equals(key, "Authorization", StringComparison.OrdinalIgnoreCase) && values.Any())
                        {
                            values = values.Select(v =>
                            {
                                var indexOfSpace = v.IndexOf(" ");
                                return (indexOfSpace > 0 ? v?.Substring(0, indexOfSpace) : string.Empty) + " **********";
                            });
                        }
                        logMessage += $"{Environment.NewLine}>>>    {key}: {string.Join(",", values)}";
                    }
                }
                if (request.Content?.Headers.Any() == true)
                {
                    foreach (var header in request.Content.Headers)
                    {
                        var key = header.Key;
                        var values = header.Value;
                        logMessage += $"{Environment.NewLine}>>>    {key}: {string.Join(",", values)}";
                    }
                }
                watch = Stopwatch.StartNew();
                return logMessage + Environment.NewLine;
            });
        }

        public void LogEnd(HttpRequestMessage request, HttpResponseMessage response)
        {
            _logger.Debug(() =>
            {
                watch.Stop();
                var logMessage = $"<<< Response [{request.Method}] {request.RequestUri.OriginalString}" +
                    $"{Environment.NewLine}<<< Duration: {watch.ElapsedMilliseconds}ms" +
                    $"{Environment.NewLine}<<< StatusCode: [{response.StatusCode}]";
                if (response.Headers.Any())
                {
                    logMessage += $"{Environment.NewLine}<<< Headers:";
                    foreach (var header in response.Headers)
                    {
                        var key = header.Key;
                        if (InterestingResponseHeaders.Contains(key))
                        {
                            // log only interesting headers
                            var values = header.Value;
                            logMessage += $"{Environment.NewLine}<<<    {key}: {string.Join(",", values)}";
                        }
                    }
                }
                if (response.Content?.Headers.Any() == true)
                {
                    foreach (var header in response.Content.Headers)
                    {
                        var key = header.Key;
                        if (InterestingResponseHeaders.Contains(key))
                        {
                            // log only interesting headers
                            var values = header.Value;
                            logMessage += $"{Environment.NewLine}<<<    {key}: {string.Join(",", values)}";
                        }
                    }
                }
                return logMessage + Environment.NewLine;
            });
        }
    }
}
