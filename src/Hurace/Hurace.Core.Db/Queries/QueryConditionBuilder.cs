using System;
using System.Collections.Generic;
using System.Linq;

namespace Hurace.Core.Db.Queries
{
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
            else if (string.IsNullOrEmpty(columnToCheck))
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
        /// Declare a condition that allows everything
        /// </summary>
        /// <returns>a reference to itself</returns>
        public QueryConditionBuilder DeclareTautologyCondition()
        {
            this.QueryCondition = new StaticQueryCondition(alwaysTrue: true);

            return this;
        }

        /// <summary>
        /// Declare a condition that allows nothing
        /// </summary>
        /// <returns>a reference to itself</returns>
        public QueryConditionBuilder DeclareContradictingCondition()
        {
            this.QueryCondition = new StaticQueryCondition(alwaysTrue: false);

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
        /// Declares a uniting-condition from a set of <see cref="QueryConditionBuilder"/>.
        /// The single conditions are joined with a <see cref="QueryConditionNodeType.And"/> condition.
        /// </summary>
        /// <param name="builderSet">the set of builders to be joined</param>
        /// <returns>a uniting <see cref="QueryConditionBuilder"/> that contains all passed
        /// <see cref="QueryConditionBuilder"/>.</returns>
        public QueryConditionBuilder DeclareConditionFromBuilderSet(
            QueryConditionNodeType joiningNodeType,
            IEnumerable<QueryConditionBuilder> builderSet)
        {
            if (builderSet is null)
                throw new ArgumentNullException(nameof(builderSet));
            else if (!builderSet.Any())
                throw new InvalidOperationException($"{nameof(builderSet)} is empty");

            QueryConditionBuilder agglomerativeConditionBuilder = null;
            foreach (var builder in builderSet)
            {
                agglomerativeConditionBuilder = agglomerativeConditionBuilder == null
                    ? builder
                    : new QueryConditionBuilder()
                        .DeclareConditionNode(
                            joiningNodeType,
                            () => agglomerativeConditionBuilder,
                            () => builder);
            }
            this.QueryCondition = agglomerativeConditionBuilder.QueryCondition;
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
