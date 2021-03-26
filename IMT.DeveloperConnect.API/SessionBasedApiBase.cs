using IMT.DeveloperConnect.API.Client.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API
{
    public abstract class SessionBasedApiBase : ApiBase
    {
        private readonly IApiSession _session;

        public SessionBasedApiBase(IApiSession session, IApiClient apiClient) : base(apiClient)
        {
            _session = ArgumentChecks.IsNotNull(session, nameof(session));
        }

        protected override async Task<IApiResponse> Send(IApiRequest request)
        {
            ArgumentChecks.IsNotNull(request, nameof(request));

            var token = await _session.GetAccessToken();
            request.AddAuthorizationHeader(token.Scheme, token.Parameter);
            return await base.Send(request);
        }
    }
}