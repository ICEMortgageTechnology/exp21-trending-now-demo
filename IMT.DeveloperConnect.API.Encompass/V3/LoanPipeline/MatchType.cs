namespace IMT.DeveloperConnect.API.Encompass.V3.LoanPipeline
{
    public enum MatchType
    {
        #region OrdinalMatchType

        Equals,
        NotEquals,
        GreaterThan,
        GreaterThanOrEquals,
        LessThan,
        LessThanOrEquals,

        #endregion

        #region StringMatchType

        Exact,
        StartsWith,
        Contains,
        MultiValue,

        #endregion

        #region Generic Match Types

        IsEmpty,
        IsNotEmpty

        #endregion
    }
}
