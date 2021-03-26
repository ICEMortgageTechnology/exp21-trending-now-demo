using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Encompass.V3.Attachments
{
    public class AttachmentUploadInputContract
    {
        public class FileData
        {
            public string ContentType { get; set; }

            public string Name { get; set; }

            public decimal Size { get; set; }
        }

        public FileData File { get; set; }

        public string Title { get; set; }
    }
}
