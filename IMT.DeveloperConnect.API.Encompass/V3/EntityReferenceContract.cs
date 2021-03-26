using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Encompass.V3
{
    public enum EntityType 
    {
        Application,
        Attachment,
        Borrower,
        BorrowerContact,
        Branch,
        BranchExtension,
        BusinessContact,
        ClosingCostTemplate,
        CoBorrower,
        Company,
        CompanyExtension,
        Condition,
        ConditionView,
        CorrespondentMaster,
        CorrespondentTrade,
        Default,
        Document,
        DocumentTemplate,
        ExternalOrg,
        FieldDefinition,
        Form,
        Loan,
        LoanDuplicationTemplate,
        LoanFolder,
        LoanTemplateSet,
        LoanTrade,
        MbsPool,
        Milestone,
        MilestoneSetting,
        NonBorrowingOwner,
        Organization,
        Persona,
        PersonaView,
        PipelineView,
        ReoProperty,
        ResourceLock,
        Role,
        TemplateFolder,
        Trade,
        User,
        UserGroup,
        UserView,
        Vod,
        Voe,
        Vol,
        Vom,
        Vor
    }

    public class EntityReferenceContract
    {
        [JsonProperty("entityId")]
        public string EntityId { get; set; }

        [JsonProperty("entityName")]
        public string EntityName { get; set; }

        [JsonProperty("entityType", NullValueHandling = NullValueHandling.Ignore)]
        public EntityType? EntityType { get; set; }

        [JsonProperty("entityUri")]
        public string EntityUri { get; set; }
    }

    public class ApplicationReferenceContract : EntityReferenceContract
    {
        [JsonProperty("legacyId")]
        public string LegacyId { get; set; }
    }

    public class FileAttachmentReferenceContract : EntityReferenceContract
    {

        [JsonProperty("isActive")]
        public bool? IsActive { get; set; }
    }
}
