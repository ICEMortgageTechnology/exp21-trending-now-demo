using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API
{
    public interface IApiSession : IDisposable
    {
        Task<AccessToken> GetAccessToken();

        T GetApi<T>() where T : SessionBasedApiBase;
    }
}
