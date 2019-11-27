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
    public class SqlQueryGenerator<T> where T : DomainObjectBase
    {
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

        public (string query, QueryParameter[] queryParameters) GenerateUpdateQuery(
            T updatedDomainObject)
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

            // validate if objectContainingChanges does not pass unrecognized properties
            foreach (var currentProperty in objectContainingChanges.GetType().GetProperties())
            {
                if (!typeof(T).GetProperties().Any(p => p.Name == currentProperty.Name))
                    throw new InvalidOperationException($"Property {currentProperty.Name} does not exist for type {typeof(T).Name}");
            }

            var queryStringBuilder = new StringBuilder();
            var queryParameters = new List<QueryParameter>();

            queryStringBuilder.Append($"UPDATE [Hurace].[{typeof(T).Name}] SET");

            AppendColumnValuesWithAssignment(queryStringBuilder, queryParameters, objectContainingChanges);

            queryStringBuilder.Append(" WHERE ");

            updateCondition.AppendTo(queryStringBuilder, queryParameters);

            return (queryStringBuilder.ToString(), queryParameters.ToArray());
        }


        public (string query, QueryParameter[] queryParameters) GenerateDeleteQuery(int id)
        {
            return this.GenerateDeleteQuery(
                new QueryCondition()
                {
                    ColumnToCheck = "Id",
                    CompareValue = id,
                    ConditionType = QueryCondition.Type.Equals
                });
        }

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
