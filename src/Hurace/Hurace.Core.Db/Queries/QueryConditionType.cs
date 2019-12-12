namespace Hurace.Core.Db.Queries
{
    /// <summary>
    /// Describes how a <see cref="QueryCondition"/> compares a column to a concrete value
    /// </summary>
    public enum QueryConditionType
    {
        Equals,
        NotEquals,
        LessThan,
        LessThanOrEquals,
        GreaterThan,
        GreaterThanOrEquals,
        Like
    }
}
