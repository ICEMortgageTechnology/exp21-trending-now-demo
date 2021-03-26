using IMT.DeveloperConnect.API.Client.Http;
using System.Security;

namespace IMT.DeveloperConnect.API.OAuth2
{
    public class UserCredentials : ITokenGrantData
    {
        private readonly string _userName;
        private readonly SecureString _password;

        public UserCredentials(string userName, SecureString password)
        {
            _userName = ArgumentChecks.IsNotNullOrEmpty(userName, nameof(userName));
            _password = ArgumentChecks.IsNotNull(password, nameof(password));
        }

        public void AddGrantData(IApiRequest request)
        {
            request.AddFormParameter("grant_type", "password")
                .AddFormParameter("username", _userName)
                .AddFormParameter("password", _password);
        }
    }
}
