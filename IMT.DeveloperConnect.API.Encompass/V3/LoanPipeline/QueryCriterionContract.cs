namespace IMT.DeveloperConnect.API.Encompass.V3.LoanPipeline
{
    public class QueryCriterionContract
    {
        public virtual string CanonicalName { get; set; }

        public virtual object Value { get; set; }

        public virtual MatchType? MatchType { get; set; }

        public virtual DateMatchPrecision? Precision { get; set; }

        public virtual bool? Include { get; set; }

        public virtual QueryCriterionContract[] Terms { get; set; }

        public virtual BinaryOperator? Operator { get; set; }
    }
}
