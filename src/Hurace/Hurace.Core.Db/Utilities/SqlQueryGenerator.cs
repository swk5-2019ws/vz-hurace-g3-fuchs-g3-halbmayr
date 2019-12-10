using Hurace.Core.Db.Extensions;
using Hurace.Core.Db.Queries;
using Hurace.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hurace.Core.Db.Utilities
{
    /// <summary>
    /// Consists of methods that generate Select, Insert, Update and Delete queries.
    /// The queries are also sanitized with <see cref="QueryParameter"/>s.
    /// </summary>
    /// <typeparam name="T">the specific domain-object type</typeparam>
    public class SqlQueryGenerator<T> where T : DomainObjectBase
    {
        /// <summary>
        /// Generates a SELECT query that generates a select query that only returns a single
        /// row that matches the passed id, if such a row exists
        /// </summary>
        /// <param name="id">id of the requested row</param>
        /// <returns>a query-<see cref="string"/> and a <see cref="QueryParameter[]"/>
        /// that contains the sanitized value of the passed id</returns>
        public (string query, QueryParameter[] queryParameters) GenerateSelectQuery(int id)
        {
            return this.GenerateSelectQuery(
                new QueryConditionBuilder()
                .DeclareCondition(nameof(DomainObjectBase.Id), QueryConditionType.Equals, id)
                .Build());
        }

        /// <summary>
        /// Generates a SELECT query that supplies the executer with either all rows, or rows
        /// that fall into passed rules
        /// </summary>
        /// <param name="selectCondition">gets transformed into a where clause</param>
        /// <returns>a <see cref="ValueTuple{string, QueryParameter[]}" containing the
        /// select query as a string and optionally the parameters of the
        /// passed condition/>a query-<see cref="string"/> and a <see cref="QueryParameter[]"/>
        /// that contains the sanitized values of the passed <see cref="IQueryCondition"/></returns>
        public (string query, QueryParameter[] queryParameters) GenerateSelectQuery(IQueryCondition selectCondition = null)
        {
            var queryStringBuilder = new StringBuilder();

            queryStringBuilder.Append("SELECT ");

            AppendColumnNames(queryStringBuilder);

            queryStringBuilder.Append($" FROM [Hurace].[{typeof(T).Name}]");

            var queryParameters = new List<QueryParameter>();
            if (selectCondition != null)
            {
                queryStringBuilder.Append(" WHERE ");

                selectCondition.AppendTo(queryStringBuilder, queryParameters);
            }

            return (queryStringBuilder.ToString(), queryParameters.ToArray());
        }

        /// <summary>
        /// Generates a INSERT query that inserts a passed domain-object into
        /// a db-table.
        /// </summary>
        /// <param name="newDomainObjct">the domain-object that should be inserted</param>
        /// <returns>a query-<see cref="string"/> and a <see cref="QueryParameter[]"/> containing
        /// the sanitized column-values that should be inserted.</returns>
        public (string query, QueryParameter[] queryParameters) GenerateInsertQuery(T newDomainObjct)
        {
            if (newDomainObjct == null) throw new ArgumentNullException(nameof(newDomainObjct));

            var queryStringBuilder = new StringBuilder();
            var queryParameters = new List<QueryParameter>();

            queryStringBuilder.Append($"INSERT INTO [Hurace].[{newDomainObjct.GetType().Name}] (");

            AppendColumnNames(queryStringBuilder, (m) => m.Name == "Id");

            queryStringBuilder.Append($") VALUES (");

            AppendColumnValuesWithoutAssignment(queryStringBuilder, queryParameters, newDomainObjct);

            queryStringBuilder.Append(")");

            return (queryStringBuilder.ToString(), queryParameters.ToArray());
        }

        /// <summary>
        /// Generates a UPDATE query that updates a specific domainobject identified by its 
        /// Id-property. The query consists of all property values of the domain-object.
        /// It is important, that the Id is not updated, since it links the updated object
        /// to a row in the db.
        /// </summary>
        /// <param name="newDomainObjct">the updated domain-object</param>
        /// <returns>a query-<see cref="string"/> and a <see cref="QueryParameter[]"/> that
        /// contains a single <see cref="QueryParameter"/> describing the sanitized Id</returns>
        public (string query, QueryParameter[] queryParameters) GenerateUpdateQuery(T updatedDomainObject)
        {
            if (updatedDomainObject == null) throw new ArgumentNullException(nameof(updatedDomainObject));
            if (updatedDomainObject.Id < 0) throw new ArgumentOutOfRangeException(nameof(updatedDomainObject));

            var queryStringBuilder = new StringBuilder();
            var queryParameters = new List<QueryParameter>();

            queryStringBuilder.Append($"UPDATE [Hurace].[{typeof(T).Name}] SET");

            AppendColumnValuesWithAssignment(queryStringBuilder, queryParameters, updatedDomainObject);

            queryStringBuilder.Append(" WHERE ");

            var idParameter = queryParameters.AddQueryParameter("Id", updatedDomainObject.Id);
            queryStringBuilder.Append($"[Id] = @{idParameter.ParameterName}");

            return (queryStringBuilder.ToString(), queryParameters.ToArray());
        }

        /// <summary>
        /// Generates a UPDATE query that applies a set of values passed with a
        /// anonymous object, to a set of rows that fulfill the passed <see cref="IQueryCondition"/>.
        /// It is mandatory to use a anonymous object that only contains the Properties
        /// that should be updated, because a passed domain-object would result in a query
        /// updating all columns and that is not the desired behaviour most of the time.
        /// </summary>
        /// <param name="objectContainingChanges">the anonymous object containig the updated values</param>
        /// <param name="updateCondition">the condition, which rows should be updated</param>
        /// <returns>a query-<see cref="string"/> and a <see cref="QueryParameter[]"/>
        /// that contains the updated values and the values used in the WHERE clause.
        /// Both types of parameters are sanitized.</returns>
        public (string query, QueryParameter[] queryParameters) GenerateUpdateQuery(
            object objectContainingChanges,
            IQueryCondition updateCondition)
        {
            if (updateCondition is null)
                throw new ArgumentNullException(nameof(updateCondition));
            if (objectContainingChanges is null)
                throw new ArgumentNullException(nameof(objectContainingChanges));
            if (objectContainingChanges.GetType() == typeof(T))
                throw new InvalidOperationException(
                    "You are not allowed to pass the domain object itself" +
                    " -> pass a anonymous object containing the changes to apply");

            foreach (var currentProperty in objectContainingChanges.GetType().GetProperties())
            {
                if (currentProperty.Name == "Id")
                    throw new InvalidOperationException(
                        "Attempted to update Id column -> forbidden operation");
                else if (!typeof(T).GetProperties().Any(p => p.Name == currentProperty.Name))
                    throw new InvalidOperationException(
                        $"Property {currentProperty.Name} does not exist for type {typeof(T).Name}");

                var correctType = typeof(T).GetProperties()
                    .First(p => p.Name == currentProperty.Name)
                    .PropertyType;

                if (!(correctType == currentProperty.PropertyType))
                    throw new InvalidOperationException(
                        $"Property-Type mismatch of property {currentProperty.Name}; " +
                        $"expected type {correctType.FullName}, actual type {currentProperty.PropertyType.FullName}");
            }

            var queryStringBuilder = new StringBuilder();
            var queryParameters = new List<QueryParameter>();

            queryStringBuilder.Append($"UPDATE [Hurace].[{typeof(T).Name}] SET");

            AppendColumnValuesWithAssignment(queryStringBuilder, queryParameters, objectContainingChanges);

            queryStringBuilder.Append(" WHERE ");

            updateCondition.AppendTo(queryStringBuilder, queryParameters);

            return (queryStringBuilder.ToString(), queryParameters.ToArray());
        }

        /// <summary>
        /// Generates a DELETE query that removes a single row from the db, that is
        /// identified by a passed id.
        /// </summary>
        /// <param name="id">the id that identifies the row to delete</param>
        /// <returns>a query-<see cref="string"/> and a <see cref="QueryParameter[]"/>
        /// that contains the sanitized id-value</returns>
        public (string query, QueryParameter[] queryParameters) GenerateDeleteQuery(int id)
        {
            return this.GenerateDeleteQuery(
                new QueryConditionBuilder()
                .DeclareCondition(nameof(DomainObjectBase.Id), QueryConditionType.Equals, id)
                .Build());
        }

        /// <summary>
        /// Generates a DELETE query that removes all rows from the db, that fulfill
        /// a passed condition.
        /// </summary>
        /// <param name="deleteCondition">if a row fulfills this condition, the generated query will delete it.</param>
        /// <returns>a query-<see cref="string"/> and a <see cref="QueryParameter[]"/> that
        /// contains all parameters used in the WHERE-clause in a sanitized way.</returns>
        public (string query, QueryParameter[] queryParameters) GenerateDeleteQuery(IQueryCondition deleteCondition)
        {
            if (deleteCondition is null)
                throw new ArgumentNullException(nameof(deleteCondition));

            var queryStringBuilder = new StringBuilder();
            var queryParameters = new List<QueryParameter>();

            queryStringBuilder.Append($"DELETE FROM [Hurace].[{typeof(T).Name}] WHERE ");

            deleteCondition.AppendTo(queryStringBuilder, queryParameters);

            return (queryStringBuilder.ToString(), queryParameters.ToArray());
        }

        /// <summary>
        /// Generates a query that returns the last generated Id for a specific table.
        /// </summary>
        /// <returns>a query-<see cref="string"/> that, when executed, returns the
        /// last generated identity for a specific table</returns>
        public string GenerateGetLastIdentityQuery()
        {
            return $"SELECT IDENT_CURRENT('[Hurace].[{typeof(T).Name}]')";
        }

        #region Helpers

        private void AppendColumnNames(StringBuilder sb, Predicate<PropertyInfo> propertyFilter = null)
        {
            bool firstProperty = true;
            foreach (var currentProperty in typeof(T).GetProperties())
            {
                if (propertyFilter == null || !propertyFilter(currentProperty))
                {
                    sb.Append($"{(firstProperty ? "" : ", ")}[{currentProperty.Name}]");
                    firstProperty = false;
                }
            }
        }

        private void AppendColumnValuesWithoutAssignment(
           StringBuilder queryStringBuilder,
           IList<QueryParameter> queryParameters,
           object domainObject)
        {
            bool firstProperty = true;

            foreach (var currentProperty in typeof(T).GetProperties())
            {
                if (currentProperty.Name != "Id")
                {
                    var queryParameter = queryParameters.AddQueryParameter(
                        currentProperty.Name,
                        currentProperty.GetValue(domainObject));

                    queryStringBuilder.Append($"{(firstProperty ? "" : ", ")}@{queryParameter.ParameterName}");

                    firstProperty = false;
                }
            }
        }

        private void AppendColumnValuesWithAssignment(
            StringBuilder queryStringBuilder,
            IList<QueryParameter> queryParameters,
            object valueContainer)
        {
            bool firstProperty = true;

            foreach (var currentProperty in valueContainer.GetType().GetProperties())
            {
                if (currentProperty.Name != "Id")
                {
                    var queryParameter = queryParameters.AddQueryParameter(
                        currentProperty.Name,
                        currentProperty.GetValue(valueContainer));

                    queryStringBuilder.Append($"{(firstProperty ? "" : ",")} [{currentProperty.Name}] = @{queryParameter.ParameterName}");

                    firstProperty = false;
                }
            }
        }

        #endregion
    }
}
