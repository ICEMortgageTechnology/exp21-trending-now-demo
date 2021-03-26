using Newtonsoft.Json;

namespace IMT.DeveloperConnect.API.EFolder.V1
{

    public enum EntityType
    {
        Attachment,
        Loan,
        SkyDrive
    }

    public class EntityReferenceContract
    {
        [JsonProperty("entityId")]
        public string EntityId { get; set; }


        [JsonProperty("entityType", NullValueHandling = NullValueHandling.Ignore)]
        public EntityType? EntityType { get; set; }

        [JsonProperty("entityUri")]
        public string EntityUri { get; set; }


    }
}
