using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hurace.Core.Db.Utilities
{
    public class SimpleSqlQueryGenerator<T>
    {
        public string GenerateGetAllQuery()
        {
            var sb = new StringBuilder();

            sb.Append("SELECT ");

            AppendDbColumnNames(sb);

            sb.Append($" FROM [Hurace].[{typeof(T).Name}]");

            return sb.ToString();
        }

        public string GenerateGetByIdQuery(int id)
        {
            var sb = new StringBuilder();

            sb.Append("SELECT ");

            AppendDbColumnNames(sb);

            sb.Append($" FROM [Hurace].[{typeof(T).Name}]");
            sb.Append($" WHERE [Id] = {id}");

            return sb.ToString();
        }

        public Tuple<string, QueryParameter[]> GenerateCreateQuery(T entity)
        {
            var sb = new StringBuilder();
            var queryParameters = new List<QueryParameter>();

            sb.Append($"INSERT INTO [Hurace].[{entity.GetType().Name}] (");

            AppendDbColumnNames(sb, (m) => m.Name == "Id");

            sb.Append($") VALUES (");

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

        public string GenerateUpdateQuery(int Id, T entity)
        {
            var sb = new StringBuilder();

            sb.Append($"UPDATE [Hurace].[{typeof(T).Name}] SET");
            bool firstValue = true;
            foreach (var currentProperty in entity.GetType().GetProperties())
            {
                if (currentProperty.Name != "Id")
                {
                    var val = entity.GetType().GetProperty(currentProperty.Name).GetValue(entity);
                    sb.Append($"{(firstValue ? "" : ",")} [{currentProperty.Name}] = {val}");
                    firstValue = false;
                }
            }
            sb.Append($" WHERE [Id] = {Id}");
            return sb.ToString();
        }

        public string GenerateDeleteByIdQuery(int Id)
        {
            //TODO: Set Skier Inaktive when there are already entries for him/her else delete Skier entry
            var sb = new StringBuilder();

            sb.Append($"DELETE FROM [Hurace].[{typeof(T).Name}] WHERE Id = {Id}");

            return sb.ToString();
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
