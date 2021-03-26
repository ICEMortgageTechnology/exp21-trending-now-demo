using System;

namespace IMT.DeveloperConnect.API.Encompass.V3.LoanPipeline
{
    public class LoanPipelineFieldContract
    {
        public virtual string CanonicalName { get; set; }

        public virtual string Header { get; set; }

        public FieldSortOrder Order { get; set; }

        public virtual String FieldId { get; set; }

        public int CompareTo(LoanPipelineFieldContract comparePart)
        {
            // A null value means that this object is greater. 
            if (comparePart == null)
            {
                return 1;
            }

            int diffVal = String.Compare(Header, comparePart.Header, StringComparison.CurrentCultureIgnoreCase);
            return (0 == diffVal && null != FieldId && null != comparePart.FieldId) ? String.Compare(FieldId, comparePart.FieldId, StringComparison.CurrentCultureIgnoreCase) : diffVal;
        }
    }
}
