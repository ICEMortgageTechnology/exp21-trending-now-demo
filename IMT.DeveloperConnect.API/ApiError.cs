using Newtonsoft.Json;
using System.Collections.Generic;

namespace IMT.DeveloperConnect.API
{
    public class ApiError
    {
        [JsonProperty("error")]
        public string Error { get; private set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; private set; }

        [JsonProperty("summary")]
        public string Summary { get; private set; }

        [JsonProperty("details")]
        public string Details { get; private set; }

        [JsonProperty("errorCode")]
        public string ErrorCode { get; private set; }

        [JsonProperty("errors")]
        public List<ApiError> Errors { get; private set; }

        [JsonProperty("additionalInfo")]
        public object AdditionalInfo { get; }
    }


}
