using IMT.DeveloperConnect.API.Client.Http;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API
{
    public abstract class ApiBase
    {
        protected readonly IApiClient _restClient;

        public ApiBase(IApiClient apiClient)
        {
            _restClient = ArgumentChecks.IsNotNull(apiClient, nameof(apiClient));
        }

        protected IApiRequest NewRequest(string url, HttpMethod method)
        {
            return _restClient.NewRequest(
                ArgumentChecks.IsNotNullOrEmpty(url, nameof(url)), method);
        }

        protected virtual Task<IApiResponse> Send(IApiRequest request)
        {
            return ArgumentChecks.IsNotNull(request, nameof(request)).SendAsync();
        }

        protected async Task<T> ExecuteNewRequest<T>(string url, HttpMethod method, Action<IApiRequest> prepareRequest = null)
        {
            using (var request = NewRequest(url, method))
            {
                prepareRequest?.Invoke(request);
                using (var response = await Send(request))
                {
                    return await response.FetchData<T>();
                }
            }
        }

        protected async Task ExecuteNewRequest(string url, HttpMethod method, Action<IApiRequest> prepareRequest = null)
        {
            using (var request = NewRequest(url, method))
            {
                prepareRequest?.Invoke(request);
                using (var response = await Send(request))
                {
                    return;
                }
            }
        }

    }
}
