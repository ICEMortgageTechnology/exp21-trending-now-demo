using IMT.DeveloperConnect.API.Client.Http.Content;
using IMT.DeveloperConnect.API.Client.Json;
using IMT.DeveloperConnect.API.Client.Logging;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Client.Http
{

    public class ApiClient : IApiClient
    {
        static ApiClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }

        public class ApiRequest : IApiRequest
        {
            private readonly ApiClient _apiClient;
            private readonly HttpRequestMessage _request;
            private IContentBuilder _contentBuilder = null;
            private readonly ApiRequestLogger _apiRequestLogger;
            private readonly ApiPayloadLogger _apiPayloadLogger;

            public ApiRequest(ApiClient client, string url, HttpMethod method)
            {
                _apiClient = client;
                _request = new HttpRequestMessage(method, url);
                _apiRequestLogger = new ApiRequestLogger();
                _apiPayloadLogger = new ApiPayloadLogger();
            }

            public HttpMethod Method => _request.Method;

            public string RequestUrl => _request.RequestUri.OriginalString;

            public IApiRequest AddAuthorizationHeader(string scheme, string parameter)
            {
                if (_request.Headers.Authorization == null)
                    _request.Headers.Authorization = new AuthenticationHeaderValue(scheme, parameter);
                return this;
            }

            public IApiRequest AddAuthorizationHeader(string schemeAndParameter)
            {
                if (_request.Headers.Authorization == null)
                    _request.Headers.Authorization = AuthenticationHeaderValue.Parse(schemeAndParameter);
                return this;
            }

            public IApiRequest AddBasicAuthorizationHeader(string username, SecureString password)
            {
                if (_request.Headers.Authorization == null)
                    _request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password.AsString()))));
                return this;
            }

            public IApiRequest AddQueryParameter(string key, string value)
            {
                var originalUrlString = _request.RequestUri.OriginalString;
                StringBuilder newUri = new StringBuilder(originalUrlString);
                var indexOfQueryStart = originalUrlString?.IndexOf("?") ?? -1;
                if (indexOfQueryStart == -1)
                {
                    newUri.Append("?");
                }
                else if (indexOfQueryStart < originalUrlString.Length - 1)
                {
                    newUri.Append("&");
                }
                newUri.Append(key).Append("=").Append(Uri.EscapeDataString(value));
                _request.RequestUri = new Uri(newUri.ToString(), UriKind.RelativeOrAbsolute);
                return this;
            }


            public IApiRequest AddFormParameter(string key, string value)
            {
                StringContentBuilder stringContentBuilder;
                if (_contentBuilder == null || !(_contentBuilder is StringContentBuilder))
                {
                    _contentBuilder = stringContentBuilder = new StringContentBuilder();
                }
                else
                {
                    stringContentBuilder = _contentBuilder as StringContentBuilder;
                }

                stringContentBuilder.AddParameter(key, value);
                return this;
            }


            public IApiRequest AddFormParameter(string key, SecureString value)
            {
                StringContentBuilder stringContentBuilder;
                if (_contentBuilder == null || !(_contentBuilder is StringContentBuilder))
                {
                    _contentBuilder = stringContentBuilder = new StringContentBuilder();
                }
                else
                {
                    stringContentBuilder = _contentBuilder as StringContentBuilder;
                }

                stringContentBuilder.AddParameter(key, value);
                return this;
            }

            public IApiRequest AddJsonContent<T>(T obj)
            {
                _contentBuilder = new ObjectContentBuilder<T>(obj);
                return this;
            }

            public IApiRequest AddFile(string filePath, string contentType = null)
            {
                _contentBuilder = new FileContentBuilder(filePath, contentType);
                return this;
            }

            private async Task<HttpResponseMessage> SendAsyncInternal()
            {
                try
                {
                    if (_contentBuilder != null)
                    {
                        _request.Content = _contentBuilder.Build();
                    }
                }
                catch (Exception ex)
                {
                    throw new ApiException(this, ex.Message, ex, RequestStatus.Aborted);
                }

                _apiRequestLogger.LogStart(_request);
                if (_contentBuilder != null)
                    _apiPayloadLogger.LogRequestPayload(_contentBuilder);
                var response = await _apiClient._client.SendAsync(_request);
                _apiRequestLogger.LogEnd(_request, response);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.Content != null
                        && string.Equals(response.Content.Headers.ContentType?.MediaType, "application/json", StringComparison.OrdinalIgnoreCase))
                    {
                        ApiError error;
                        try
                        {
                            using (var stream = await response.Content.ReadAsStreamAsync())
                            {
                                _apiPayloadLogger.LogResponsePayload(stream);
                                using (JsonTextReader jsonTextReader = new JsonTextReader(new StreamReader(stream)))
                                {
                                    JsonSerializer jsonSerializer = JsonSerializer.Create(JsonConstants.JsonSerializerSettings);
                                    error = jsonSerializer.Deserialize<ApiError>(jsonTextReader);
                                }
                            }
                        }
                        catch
                        {
                            throw new ApiException(this, "Request Failed with status code: " + response.StatusCode.ToString(), response.StatusCode, RequestStatus.Failed);
                        }

                        throw new ApiException(this, "Request Failed with status code: " + response.StatusCode.ToString(), response.StatusCode, error, RequestStatus.Failed);
                    }

                }
                return response;
            }

            public async Task<IApiResponse> SendAsync()
            {
                try
                {
                    return new ApiResponse(this, await SendAsyncInternal(), _apiPayloadLogger);
                }
                catch (Exception exception)
                {
                    if (exception is ApiException)
                        throw exception;
                    throw new ApiException(this, exception.Message, exception, RequestStatus.Failed);
                }
            }

            public void Dispose()
            {
                _request.Dispose();
            }
        }

        private class ApiResponse : IApiResponse
        {
            private readonly IApiRequest _apiRequest;
            private readonly HttpResponseMessage _response;
            private readonly ApiPayloadLogger _apiPayloadLogger;

            public ApiResponse(IApiRequest apiRequest, HttpResponseMessage response, ApiPayloadLogger apiPayloadLogger)
            {
                _apiRequest = apiRequest;
                _response = response;
                requestStatus = RequestStatus.Completed;
                _apiPayloadLogger = apiPayloadLogger;
            }

            public void Dispose()
            {
                _response.Dispose();
            }

            public RequestStatus requestStatus { get; }

            public HttpStatusCode StatusCode => _response?.StatusCode ?? 0;

            public Exception Exception { get; private set; }

            public Task<Stream> ReadContentAsStreamAsync()
            {
                return _response.Content.ReadAsStreamAsync();
            }

            public async Task<T> FetchData<T>()
            {
                try
                {
                    using (var stream = await ReadContentAsStreamAsync())
                    {
                        _apiPayloadLogger.LogResponsePayload(stream);
                        using (JsonTextReader jsonTextReader = new JsonTextReader(new StreamReader(stream)))
                        {
                            JsonSerializer jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings());
                            return jsonSerializer.Deserialize<T>(jsonTextReader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ApiException(_apiRequest, "Request was successful but there was an error while deserializing response data", _response.StatusCode, ex, RequestStatus.Error);
                }
            }
        }


        private readonly HttpClient _client;

        public ApiClient(string hostUrl) : base()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(hostUrl);
        }

        public IApiRequest NewRequest(string url, HttpMethod method)
        {
            return new ApiRequest(this, url, method);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }

}
