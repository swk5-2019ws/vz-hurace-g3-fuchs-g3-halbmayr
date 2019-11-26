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
    public class SimpleSqlQueryGenerator<T> where T : DomainObjectBase
    {
        private enum Context
        {
            Create,
            Update
        }

        public string GenerateGetAllConditionalQuery(IQueryCondition condition = null)
        {
            var sb = new StringBuilder();

            sb.Append("SELECT ");

            AppendPropertiesAsColumnNames(sb);

            sb.Append($" FROM [Hurace].[{typeof(T).Name}]");

            if (condition != null)
            {
                sb.Append(" WHERE ");
                condition.Build(sb);
            }

            return sb.ToString();
        }

        public Tuple<string, QueryParameter[]> GenerateGetByIdQuery(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));

            var sb = new StringBuilder();
            var queryParameters = new List<QueryParameter>();

            sb.Append("SELECT ");

            AppendPropertiesAsColumnNames(sb);

            sb.Append($" FROM [Hurace].[{typeof(T).Name}]");

            sb.Append($" WHERE [Id] = @Id");

            queryParameters.Add(
                new QueryParameter("Id", id));

            return Tuple.Create(sb.ToString(), queryParameters.ToArray());
        }

        public Tuple<string, QueryParameter[]> GenerateCreateQuery(T newDomainObjct)
        {
            if (newDomainObjct == null) throw new ArgumentNullException(nameof(newDomainObjct));

            var sb = new StringBuilder();
            var queryParameters = new List<QueryParameter>();

            sb.Append($"INSERT INTO [Hurace].[{newDomainObjct.GetType().Name}] (");

            AppendPropertiesAsColumnNames(sb, (m) => m.Name == "Id");

            sb.Append($") VALUES (");

            queryParameters = AppendPropertiesAndGetValues(sb, Context.Create, newDomainObjct);

            sb.Append(")");

            return Tuple.Create(sb.ToString(), queryParameters.ToArray());
        }

        public Tuple<string, QueryParameter[]> GenerateUpdateQuery(T updatedDomainObject)
        {
            if (updatedDomainObject == null) throw new ArgumentNullException(nameof(updatedDomainObject));
            if (updatedDomainObject.Id < 0) throw new ArgumentOutOfRangeException(nameof(updatedDomainObject));

            var sb = new StringBuilder();
            var queryParameters = new List<QueryParameter>();

            sb.Append($"UPDATE [Hurace].[{typeof(T).Name}] SET");

            queryParameters = AppendPropertiesAndGetValues(sb, Context.Update, updatedDomainObject);

            sb.Append($" WHERE [Id] = @Id");

            queryParameters.Add(
                        new QueryParameter("Id", updatedDomainObject.Id));
            return Tuple.Create(sb.ToString(), queryParameters.ToArray());
        }


        public Tuple<string, QueryParameter[]> GenerateDeleteByIdQuery(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));

            var sb = new StringBuilder();
            var queryParameters = new List<QueryParameter>();

            sb.Append($"DELETE FROM [Hurace].[{typeof(T).Name}] WHERE Id = @Id");

            queryParameters.Add(
                        new QueryParameter("Id", id));

            return Tuple.Create(sb.ToString(), queryParameters.ToArray());
        }

        public string GenerateGetLastIdentityQuery()
        {
            return $"SELECT IDENT_CURRENT('[Hurace].[{typeof(T).Name}]')";
        }

        private void AppendPropertiesAsColumnNames(StringBuilder sb, Predicate<PropertyInfo> propertyFilter = null)
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

        private List<QueryParameter> AppendPropertiesAndGetValues(
            StringBuilder sb,
            Context context,
            T domainObject,
            Predicate<PropertyInfo> propertyFilter = null)
        {
            List<QueryParameter> queryParameters = new List<QueryParameter>();
            bool firstProperty = true;


            foreach (var currentProperty in typeof(T).GetProperties())
            {
                if (currentProperty.Name != "Id" && (propertyFilter == null || !propertyFilter(currentProperty)))
                {
                    if (context == Context.Create)
                        sb.Append($"{(firstProperty ? "" : ", ")}@{currentProperty.Name}");
                    else
                        sb.Append($"{(firstProperty ? "" : ",")} [{currentProperty.Name}] = @{currentProperty.Name}");

                    queryParameters.Add(
                            new QueryParameter(currentProperty.Name, currentProperty.GetValue(domainObject)));

                    firstProperty = false;
                }
            }

            return queryParameters;
        }
    }
}
