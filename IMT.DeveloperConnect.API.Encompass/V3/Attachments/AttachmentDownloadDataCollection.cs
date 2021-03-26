using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Encompass.V3.Attachments
{
    public class AttachmentDownloadDataCollection
    {

        public AttachmentDownloadUrlDataContract[] Attachments { get; set; }
    }

    public class AttachmentDownloadUrlDataContract
    {
        public string Id { get; set; }

        public string AuthorizationHeader { get; set; }

        public string Url { get; set; }

    }

}
