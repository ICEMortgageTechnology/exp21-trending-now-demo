namespace IMT.DeveloperConnect.API.EFolder.V1.Export
{
    public class ExportJobStatusContract
    {
        public string JobId { get; set; }

        public string Status { get; set; }

        public ExportFileReferenceContract File { get; set; }
    }
}
