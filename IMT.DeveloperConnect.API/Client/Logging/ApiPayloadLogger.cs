using IMT.DeveloperConnect.API.Client.Http.Content;
using IMT.DeveloperConnect.API.Client.Json;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Client.Logging
{
    public class ApiPayloadLogger
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void LogResponsePayload(Stream stream)
        {
            _logger.Debug(() =>
            {
                if (stream.CanSeek)
                {
                    string strValue;
                    stream.Seek(0, SeekOrigin.Begin);
                    using (StreamReader rdr = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
                        strValue = rdr.ReadToEnd();
                    stream.Seek(0, SeekOrigin.Begin);

                    // Not the best way for logging but does the magic when demoing the logs in a console window.
                    try
                    {
                        strValue = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(strValue), Formatting.Indented, JsonConstants.JsonSerializerSettings);
                    }
                    catch
                    { /* not json */ }

                    return $"<<< Response Payload: {Environment.NewLine}{strValue}{Environment.NewLine}";
                }
                else
                {
                    return "(Request payload cannot be read for logging because data stream does not support Seeking.)" + Environment.NewLine;
                }
            });
        }

        public void LogRequestPayload(IContentBuilder contentBuilder)
        {
            _logger.Debug(() =>
            {
                return $">>> Request Payload: {Environment.NewLine}{contentBuilder.GetPayloadForLogging()}{Environment.NewLine}";
            });
        }
    }
}
