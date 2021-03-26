using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Client.Json
{
    public class JsonConstants
    {
        static JsonConstants()
        {
            JsonSerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            JsonSerializerSettings.Converters.Add(new StringEnumConverter());
        }

        public static JsonSerializerSettings JsonSerializerSettings { get; }
    }
}
