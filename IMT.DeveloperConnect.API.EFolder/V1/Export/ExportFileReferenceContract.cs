namespace IMT.DeveloperConnect.API.EFolder.V1.Export
{
    public class ExportFileReferenceContract : EntityReferenceContract
    {
        public string AuthorizationHeader { get; set; }

        public string ContentType { get; set; }

        public long FileSize { get; set; }

        public int PageCount { get; set; }
    }
}
