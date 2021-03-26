using IMT.DeveloperConnect.API.Client.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;

namespace IMT.DeveloperConnect.API.Client.Http.Content
{
    public class StringContentBuilder : IContentBuilder
    {
        private readonly List<KeyValuePair<string, object>> _builder = new List<KeyValuePair<string, object>>();

        public void AddParameter(string key, string value)
        {
            _builder.Add(new KeyValuePair<string, object>(key, value));
        }

        public void AddParameter(string key, SecureString value)
        {
            _builder.Add(new KeyValuePair<string, object>(key, value));
        }

        public HttpContent Build()
        {
            var content = new StringContent(string.Join("&", _builder.Select(item =>
            {
                return $"{item.Key}={GetStringValue(item.Value)}";
            })));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            return content;
        }

        private string GetStringValue(object value)
        {
            if (value is SecureString)
                return (value as SecureString).AsString();
            else if (value is string)
                return value as string;
            else
                return null;
        }

        public string GetPayloadForLogging()
        {

            return string.Join("&", _builder.Select(item =>
            {
                return $"{item.Key}={(item.Value is string ? item.Value as string : "******")}";
            }));
        }

    }
}
