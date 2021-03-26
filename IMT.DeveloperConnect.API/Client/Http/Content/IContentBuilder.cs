using System.Net.Http;

namespace IMT.DeveloperConnect.API.Client.Http.Content
{
    public interface IContentBuilder
    {
        HttpContent Build();

        string GetPayloadForLogging();
    }
}
