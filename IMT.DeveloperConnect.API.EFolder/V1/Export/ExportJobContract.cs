namespace IMT.DeveloperConnect.API.EFolder.V1.Export
{
    public class ExportJobContract
    {
        public AnnotationSettingsContract AnnotationSettings { get; set; }

        public EntityReferenceContract[] Entities { get; set; }

        public EntityReferenceContract Source { get; set; }
    }
}
