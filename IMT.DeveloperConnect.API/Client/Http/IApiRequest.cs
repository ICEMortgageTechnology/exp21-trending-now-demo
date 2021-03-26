using System;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Client.Http
{
    public interface IApiRequest : IDisposable
    {
        HttpMethod Method { get; }

        string RequestUrl { get; }

        IApiRequest AddAuthorizationHeader(string scheme, string parameter);

        IApiRequest AddAuthorizationHeader(string schemeAndParameter);

        IApiRequest AddBasicAuthorizationHeader(string username, SecureString password);

        IApiRequest AddQueryParameter(string key, string value);

        IApiRequest AddFormParameter(string key, string value);

        IApiRequest AddFormParameter(string key, SecureString value);

        IApiRequest AddJsonContent<T>(T obj);

        IApiRequest AddFile(string filePath, string contentType = null);

        Task<IApiResponse> SendAsync();

    }


}
