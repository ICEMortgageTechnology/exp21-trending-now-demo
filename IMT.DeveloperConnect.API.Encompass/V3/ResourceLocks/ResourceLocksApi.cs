using IMT.DeveloperConnect.API.Client.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Encompass.V3.ResourceLocks
{
    public class ResourceLocksApi : SessionBasedApiBase
    {
        private const string RespurceLocksUrl = "/encompass/v3/resourceLocks";
        private const string ResourceLockUrl = "/encompass/v3/resourceLocks/{0}";

        public ResourceLocksApi(IApiSession session, IApiClient apiClient) : base(session, apiClient)
        {
        }

        public async Task<IDisposable> CreateResourceLock(ResourceLockContract input)
        {
            ArgumentChecks.IsNotNull(input, nameof(input));

            return new ResourceLock(this, 
                await ExecuteNewRequest<ResourceLockContract>(
                    RespurceLocksUrl, 
                    Method.Post, 
                    request =>
                    {
                        request.AddJsonContent(input);
                        request.AddQueryParameter("view", "entity");
                    }));
        }

        public async Task DeleteResourceLock(ResourceLockContract input)
        {
            ArgumentChecks.IsNotNull(input, nameof(input));

            await ExecuteNewRequest(
                string.Format(ResourceLockUrl, input.Id),
                Method.Delete,
                request =>
                {
                    request.AddQueryParameter("resourceType", input.Resource.EntityType.ToString());
                    request.AddQueryParameter("resourceId", input.Resource.EntityId);
                });
        }
    }
}
