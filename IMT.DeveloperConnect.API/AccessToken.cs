using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API
{
    public class AccessToken
    {
        [JsonProperty("access_token")]
        public string Parameter { get; set; }

        [JsonProperty("token_type")]
        public string Scheme { get; set; }
    }
}
