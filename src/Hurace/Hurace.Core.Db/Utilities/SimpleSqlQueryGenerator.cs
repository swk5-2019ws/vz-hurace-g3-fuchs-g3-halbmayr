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
        public string GenerateGetAllQuery()
        {
            var sb = new StringBuilder();

            sb.Append("SELECT ");

            AppendDbColumnNames(sb);

            sb.Append($" FROM [Hurace].[{typeof(T).Name}]");

            return sb.ToString();
        }

        public Tuple<string, QueryParameter[]> GenerateGetByIdQuery(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));

            var sb = new StringBuilder();
            var queryParameters = new List<QueryParameter>();

            sb.Append("SELECT ");

            AppendDbColumnNames(sb);

            sb.Append($" FROM [Hurace].[{typeof(T).Name}]");

            sb.Append($" WHERE [Id] = @Id");

            queryParameters.Add(
                new QueryParameter("Id", id));

            return Tuple.Create(sb.ToString(), queryParameters.ToArray());
        }

        public Tuple<string, QueryParameter[]> GenerateCreateQuery(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var sb = new StringBuilder();
            var queryParameters = new List<QueryParameter>();

            sb.Append($"INSERT INTO [Hurace].[{entity.GetType().Name}] (");

            AppendDbColumnNames(sb, (m) => m.Name == "Id");

            sb.Append($") OUTPUT Inserted.ID VALUES (");

            bool firstValue = true;
            foreach (var currentProperty in entity.GetType().GetProperties())
            {
                if (currentProperty.Name != "Id")
                {
                    sb.Append($"{(firstValue ? "" : ", ")}@{currentProperty.Name}");

                    queryParameters.Add(
                        new QueryParameter(currentProperty.Name, currentProperty.GetValue(entity)));

                    firstValue = false;
                }
            }
            sb.Append(")");

            return Tuple.Create(sb.ToString(), queryParameters.ToArray());
        }

        public Tuple<string, QueryParameter[]> GenerateUpdateQuery(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entity.Id < 0) throw new ArgumentOutOfRangeException(nameof(entity));

            var sb = new StringBuilder();
            var queryParameters = new List<QueryParameter>();

            sb.Append($"UPDATE [Hurace].[{typeof(T).Name}] SET");
            bool firstValue = true;
            foreach (var currentProperty in entity.GetType().GetProperties())
            {
                if (currentProperty.Name != "Id")
                {
                    sb.Append($"{(firstValue ? "" : ",")} [{currentProperty.Name}] = @{currentProperty.Name}");

                    queryParameters.Add(
                        new QueryParameter(currentProperty.Name, currentProperty.GetValue(entity)));

                    firstValue = false;
                }
            }
            sb.Append($" WHERE [Id] = @Id");

            queryParameters.Add(
                        new QueryParameter("Id", entity.Id));
            return Tuple.Create(sb.ToString(), queryParameters.ToArray());
        }

        //TODO: Set Skier Inaktive when there are already entries for him/her else delete Skier entry
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

        private void AppendDbColumnNames(StringBuilder sb, Predicate<PropertyInfo> propertyFilter = null)
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
    }
}
