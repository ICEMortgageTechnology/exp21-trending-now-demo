using IMT.DeveloperConnect.API.Client.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.OAuth2
{
    public class TokenApi : ApiBase
    {
        private const string CreateUrl = "/oauth2/v1/token";
        private const string RevocationUrl = "/oauth2/v1/token/revocation";

        public TokenApi(IApiClient apiClient) : base(apiClient)
        {
        }

        public Task<AccessToken> Create(ClientCredentials input, ITokenGrantData grantData)
        {
            ArgumentChecks.IsNotNull(input, nameof(input));
            ArgumentChecks.IsNotNull(grantData, nameof(grantData));

            return ExecuteNewRequest<AccessToken>(CreateUrl, Method.Post, request =>
            {
                input.AddAuthHeader(request);
                grantData.AddGrantData(request);
            });
        }

        public async Task Delete(ClientCredentials input, AccessToken token)
        {
            ArgumentChecks.IsNotNull(input, nameof(input));
            ArgumentChecks.IsNotNull(token, nameof(token));

            await ExecuteNewRequest(
                RevocationUrl,
                Method.Post,
                request =>
                {
                    input.AddAuthHeader(request);
                    request.AddFormParameter("token", token.Parameter);
                });
        }
    }
}
