using System;

namespace Hurace.Core.Db.Queries
{
    class Test
    {
        void TestMethod()
        {
            var condition = new QueryConditionNode
            {
                FirstCondition = new QueryCondition
                {
                    ColumnToCheck = nameof(Domain.Country.Name),
                    CompareValue = "Austria",
                    ConditionType = QueryConditionType.Equals
                },
                NodeType = QueryConditionNodeType.And,
                SecondCondition = new QueryCondition
                {
                    ColumnToCheck = nameof(Domain.Race.Date),
                    CompareValue = DateTime.Now.Date,
                    ConditionType = QueryConditionType.LessThan
                }
            };

            var builtCondition = new QueryConditionBuilder()
                .DeclareConditionNode(
                    QueryConditionNodeType.And,
                    () => new QueryConditionBuilder()
                        .DeclareCondition(nameof(Domain.Country.Name), QueryConditionType.Equals, "Austria"),
                    () => new QueryConditionBuilder()
                        .DeclareCondition(nameof(Domain.Race.Date), QueryConditionType.LessThan, DateTime.Now.Date))
                .Build();
        }
    }

    /// <summary>
    /// A component that simplifies condition building and detects generation errors
    /// early on.
    /// </summary>
    public class QueryConditionBuilder
    {
        private IQueryCondition QueryCondition { get; set; }

        /// <summary>
        /// Declares a new <see cref="QueryCondition"/> with the passed parameters.
        /// This is only possible, if <see cref="DeclareCondition(string, QueryConditionType, object)"/> or
        /// <see cref="DeclareConditionNode(QueryConditionNodeType, Func{QueryConditionBuilder}, Func{QueryConditionBuilder})"/>
        /// have not yet been called.
        /// </summary>
        /// <param name="columnToCheck">the column to check against</param>
        /// <param name="conditionType">how should the column be compared against the compareValue</param>
        /// <param name="compareValue">the value used for comparing with the value in the column</param>
        /// <returns>a reference of itself</returns>
        public QueryConditionBuilder DeclareCondition(
            string columnToCheck,
            QueryConditionType conditionType,
            object compareValue)
        {
            if (this.QueryCondition != null)
                throw new InvalidOperationException("Can't declare more than 1 condition on this layer");
            else if (columnToCheck == null)
                throw new ArgumentNullException(nameof(columnToCheck));
            else if (compareValue == null)
                throw new ArgumentNullException(nameof(compareValue));

            this.QueryCondition = new QueryCondition()
            {
                ColumnToCheck = columnToCheck,
                CompareValue = compareValue,
                ConditionType = conditionType
            };

            return this;
        }

        /// <summary>
        /// Declares a new <see cref="QueryConditionNode"/> with the passed parameters.
        /// This is only possible, if <see cref="DeclareCondition(string, QueryConditionType, object)"/> or
        /// <see cref="DeclareConditionNode(QueryConditionNodeType, Func{QueryConditionBuilder}, Func{QueryConditionBuilder})"/>
        /// have not yet been called.
        /// </summary>
        /// <param name="nodeType">defines how the two resulting conditionBuilder are logically 
        /// evaluated together</param>
        /// <param name="leftConditionBuilder">a delegate that returns the <see cref="QueryConditionBuilder"/>
        /// for the LEFT condition sub-tree</param>
        /// <param name="rightConditionBuilder">a delegate that returns the <see cref="QueryConditionBuilder"/>
        /// for the RIGHT condition sub-tree</param>
        /// <returns>a reference of itself</returns>
        public QueryConditionBuilder DeclareConditionNode(
            QueryConditionNodeType nodeType,
            Func<QueryConditionBuilder> leftConditionBuilder,
            Func<QueryConditionBuilder> rightConditionBuilder)
        {
            if (this.QueryCondition != null)
                throw new InvalidOperationException("Can't declare more than 1 condition on this layer");
            else if (leftConditionBuilder is null)
                throw new ArgumentNullException(nameof(leftConditionBuilder));
            else if (rightConditionBuilder is null)
                throw new ArgumentNullException(nameof(rightConditionBuilder));

            this.QueryCondition = new QueryConditionNode()
            {
                NodeType = nodeType,
                FirstCondition = leftConditionBuilder().Build(),
                SecondCondition = rightConditionBuilder().Build()
            };

            return this;
        }

        /// <summary>
        /// Acquire the resulting <see cref="IQueryCondition"/>
        /// </summary>
        /// <returns>the built <see cref="IQueryCondition"/> instance</returns>
        public IQueryCondition Build()
        {
            if (this.QueryCondition == null)
                throw new InvalidOperationException(
                    $"Nothing to build -> declare a condition first with the {nameof(DeclareCondition)}- " +
                    $"or {nameof(DeclareConditionNode)}-method");

            return this.QueryCondition;
        }
    }
}
