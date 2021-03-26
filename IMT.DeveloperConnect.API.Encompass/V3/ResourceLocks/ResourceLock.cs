using System;

namespace IMT.DeveloperConnect.API.Encompass.V3.ResourceLocks
{
    public class ResourceLock : IDisposable
    {
        private readonly ResourceLocksApi _resourceLockApi;
        private readonly ResourceLockContract _resourceLockContract;

        public ResourceLock(ResourceLocksApi resourceLockApi, ResourceLockContract resourceLockContract)
        {
            _resourceLockApi = resourceLockApi;
            _resourceLockContract = resourceLockContract;
        }

        public string LockId => _resourceLockContract.Id;

        public void Dispose()
        {
            _resourceLockApi.DeleteResourceLock(_resourceLockContract).Wait();
        }
    }
}
