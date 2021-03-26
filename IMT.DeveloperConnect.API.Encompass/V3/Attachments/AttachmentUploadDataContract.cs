using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Encompass.V3.Attachments
{
    public class AttachmentUploadDataContract
    {
        public string AttachmentId { get; set; }

        public string AuthorizationHeader { get; set; }

        public bool MultiChunkRequired { get; set; }

        public string UploadUrl { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
