using IMT.DeveloperConnect.API.Client.Http;

namespace IMT.DeveloperConnect.API.OAuth2
{
    public interface ITokenGrantData
    {
        void AddGrantData(IApiRequest request);
    }
}
