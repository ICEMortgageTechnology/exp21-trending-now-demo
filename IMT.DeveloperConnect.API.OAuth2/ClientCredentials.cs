using IMT.DeveloperConnect.API.Client.Http;
using System;
using System.Security;
using System.Text;

namespace IMT.DeveloperConnect.API.OAuth2
{
    public class ClientCredentials
    {
        private readonly string _clientId;
        private readonly SecureString _clientSecret;

        public ClientCredentials(string clientId, SecureString clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public void AddAuthHeader(IApiRequest request)
        {
            request.AddBasicAuthorizationHeader(_clientId, _clientSecret);
        }
    }
}
