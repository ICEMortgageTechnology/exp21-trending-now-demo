using System;
using System.IO;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Client.Http
{
    public interface IApiResponse : IDisposable
    {
        Task<Stream> ReadContentAsStreamAsync();

        Task<T> FetchData<T>();
    }

}
