using System;
using System.Collections.Generic;

namespace IMT.DeveloperConnect.API.Encompass.V3.LoanPipeline
{
    public class LoanPipelineItemContract
    {
        public string LoanId { get; set; }


        public Dictionary<string, string> Fields { get; set; }
    }
}
