using System;

namespace IMT.DeveloperConnect.API.Encompass.V3.ResourceLocks
{
    public class ResourceLockContract
    {
        public virtual string Id { get; set; }

        public virtual EntityReferenceContract Resource { get; set; }

        public virtual string UserId { get; set; }

        public virtual ResourceLockType LockType { get; set; }

        public virtual DateTime? LockTime { get; set; }
    }
}
