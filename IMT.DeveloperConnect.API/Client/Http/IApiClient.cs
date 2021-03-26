using System;
using System.Net.Http;

namespace IMT.DeveloperConnect.API.Client.Http
{
    public interface IApiClient : IDisposable
    {
        IApiRequest NewRequest(string url, HttpMethod method);
    }


}
