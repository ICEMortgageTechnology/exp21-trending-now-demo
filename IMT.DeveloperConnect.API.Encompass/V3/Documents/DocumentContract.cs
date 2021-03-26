using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Encompass.V3.Documents
{
    public class DocumentContract
    {
        [JsonProperty("accessibleTo")]
        public string[] AccessibleTo { get; set; }

        [JsonProperty("application")]
        public ApplicationReferenceContract Application { get; set; }

        [JsonProperty("attachments")]
        public FileAttachmentReferenceContract[] Attachments { get; set; }

        [JsonProperty("conditions")]
        public EntityReferenceContract[] Conditions { get; set; }

        [JsonProperty("createdBy")]
        public EntityReferenceContract CreatedBy { get; set; }

        [JsonProperty("createdDate")]
        public DateTimeOffset? CreatedDate { get; set; }

        [JsonProperty("daysDue")]
        public long? DaysDue { get; set; }

        [JsonProperty("daysTillExpire", NullValueHandling = NullValueHandling.Ignore)]
        public long? DaysTillExpire { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("documentGroups")]
        public string[] DocumentGroups { get; set; }

        [JsonProperty("documentTypes")]
        public string[] DocumentTypes { get; set; }

        [JsonProperty("emnSignature")]
        public string EmnSignature { get; set; }

        [JsonProperty("expectedDate")]
        public DateTimeOffset? ExpectedDate { get; set; }

        [JsonProperty("expirationDate")]
        public DateTimeOffset? ExpirationDate { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("isMarkedRemoved", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsMarkedRemoved { get; set; }

        [JsonProperty("isProtected")]
        public bool? IsProtected { get; set; }

        [JsonProperty("milestone")]
        public EntityReferenceContract Milestone { get; set; }

        [JsonProperty("readyForUwBy")]
        public EntityReferenceContract ReadyForUwBy { get; set; }

        [JsonProperty("readyForUwDate")]
        public DateTimeOffset? ReadyForUwDate { get; set; }

        [JsonProperty("readyToShipBy")]
        public EntityReferenceContract ReadyToShipBy { get; set; }

        [JsonProperty("readyToShipDate")]
        public DateTimeOffset? ReadyToShipDate { get; set; }

        [JsonProperty("receivedBy")]
        public EntityReferenceContract ReceivedBy { get; set; }

        [JsonProperty("receivedDate")]
        public DateTimeOffset? ReceivedDate { get; set; }

        [JsonProperty("requestedBy")]
        public EntityReferenceContract RequestedBy { get; set; }

        [JsonProperty("requestedDate")]
        public DateTimeOffset? RequestedDate { get; set; }

        [JsonProperty("requestedFrom")]
        public string RequestedFrom { get; set; }

        [JsonProperty("rerequestedBy")]
        public EntityReferenceContract RerequestedBy { get; set; }

        [JsonProperty("rerequestedDate")]
        public DateTimeOffset? RerequestedDate { get; set; }

        [JsonProperty("reviewedBy")]
        public EntityReferenceContract ReviewedBy { get; set; }

        [JsonProperty("reviewedDate")]
        public DateTimeOffset? ReviewedDate { get; set; }

        [JsonProperty("roles")]
        public EntityReferenceContract[] Roles { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("statusDate")]
        public DateTimeOffset? StatusDate { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("verification")]
        public EntityReferenceContract Verification { get; set; }
    }
}
