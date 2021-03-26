using System;
using System.Collections.Generic;

namespace IMT.DeveloperConnect.API.Encompass.V3.LoanPipeline
{
    public class LoanPipelineQueryContract
    {
        public virtual List<Guid> LoanIds { get; set; }

        public virtual List<string> Fields { get; set; }

        public virtual List<LoanPipelineFieldContract> SortOrder { get; set; }

        public virtual QueryCriterionContract Filter { get; set; }

        public virtual LoanPipelineViewOrgType? OrgType { get; set; }

        public virtual EntityReferenceContract TpoId { get; set; }

        public virtual LoanPipelineViewOwnership? LoanOwnership { get; set; }

    }
}
