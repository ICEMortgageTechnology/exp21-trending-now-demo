using IMT.DeveloperConnect.API.Client.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.OAuth2
{
    public class OAuth2Session : IApiSession
    {
        private readonly ClientCredentials _credentials;
        private readonly IApiClient _restClient;
        private readonly ITokenGrantData _grantData;
        private readonly TokenApi _tokenApi;
        private Task _onAuthenticate;
        private DateTime _tokenLastUsed = DateTime.MinValue;
        private bool _tokenRefreshInProcess = false;
        private AccessToken _cacedToken;

        public OAuth2Session(string hostUrl, ClientCredentials clientCredentials, ITokenGrantData grantData)
        {
            _credentials = clientCredentials;
            _grantData = grantData;
            _restClient = new ApiClient(hostUrl);
            _tokenApi = new TokenApi(_restClient);
            RefreshToken();
        }

        public void RefreshToken()
        {
            lock (this)
            {
                var now = DateTime.Now;
                if (now - _tokenLastUsed > TimeSpan.FromSeconds(870)) // since token expires in 15 min refresh when unused for more than 14min 30sec
                {
                    if (!_tokenRefreshInProcess)
                    {
                        _tokenRefreshInProcess = true;
                        _cacedToken = null;
                        _onAuthenticate = _tokenApi.Create(_credentials, _grantData).ContinueWith(data =>
                        {
                            _tokenRefreshInProcess = false;
                            _tokenLastUsed = DateTime.Now;
                            _cacedToken = data.Result;
                        });
                    }
                    //else - nothing to do here other than waiting for existing refresh token in progress
                }
                else
                {
                    _tokenLastUsed = now;
                }
            }
        }

        public void Dispose()
        {
            _onAuthenticate.Wait();
            _tokenApi.Delete(_credentials, _cacedToken).Wait();
            _cacedToken = null;
            _onAuthenticate = null;
        }

        public T GetApi<T>() where T : SessionBasedApiBase
        {
            return Activator.CreateInstance(typeof(T), this, _restClient) as T;
        }

        public async Task<AccessToken> GetAccessToken()
        {
            RefreshToken();
            await _onAuthenticate;
            return _cacedToken;
        }
    }
}
