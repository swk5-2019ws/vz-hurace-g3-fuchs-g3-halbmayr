namespace Hurace.Core.Db.Queries
{
    /// <summary>
    /// Describes how two <see cref="IQueryCondition"/>s held by this instance are logically
    /// evaluated together.
    /// </summary>
    public enum QueryConditionNodeType
    {
        And,
        Or
    }
}
